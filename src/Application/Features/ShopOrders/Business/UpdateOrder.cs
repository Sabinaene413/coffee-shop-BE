using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Inventories;
using MyCoffeeShop.Application.Transactions;
using MyCoffeeShop.Application.TransactionTypes;
using MyCoffeeShop.Application.Common.Interfaces;

namespace MyCoffeeShop.Application.ShopOrders;

public record UpdateOrderCommand(
    long Id,
    string Supplier,
    decimal Cost,
    DateTime? OrderDate,
    DateTime? ArrivalDate,
    bool Received,
    List<ShopProductOrderDto> ShopOrderProducts
) : IRequest<ShopOrderDto>;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(v => v.Supplier).NotEmpty();
    }
}

internal sealed class UpdateOrderCommandHandler
    : IRequestHandler<UpdateOrderCommand, ShopOrderDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccesorService _httpContextAccesorService;

    public UpdateOrderCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper,
        IHttpContextAccesorService httpContextAccesorService

    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
        _httpContextAccesorService = httpContextAccesorService;
    }

    public async Task<ShopOrderDto> Handle(
        UpdateOrderCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.ShopOrders.Include(x => x.ShopOrderProducts)
                                      .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
               ?? throw new NotFoundException(nameof(ShopOrder), request.Id);

        var oldReceived = entity.Received;

        var deletedProducts = entity.ShopOrderProducts.Where(x => !(request.ShopOrderProducts.Where(y => y.Id.HasValue)?.Select(y => y.Id).ToList() ?? new List<long?>()).Contains(x.Id)).ToList();
        _applicationDbContext.ShopProductOrders.RemoveRange(deletedProducts);

        var newProducts = request.ShopOrderProducts.Where(x => !x.Id.HasValue)?.Select(x => new ShopProductOrder()
        {
            ShopProductId = x.ShopProductId,
            Quantity = x.Quantity,
            Price = x.Price,
            Cost = x.Cost,
            ShopOrder = entity
        }).ToList();
        await _applicationDbContext.ShopProductOrders.AddRangeAsync(newProducts, cancellationToken);

        request.ShopOrderProducts.Where(x => x.Id.HasValue)?.ToList().ForEach(product =>
        {
            var modifiedProduct = entity.ShopOrderProducts.Where(x => x.Id == product.Id).FirstOrDefault();
            modifiedProduct.Price = product.Price;
            modifiedProduct.Cost = product.Cost;
            modifiedProduct.Quantity = product.Quantity;
        });

        entity.Supplier = request.Supplier;
        entity.Cost = request.Cost;
        entity.OrderDate = request.OrderDate;
        entity.ArrivalDate = request.ArrivalDate;
        entity.Received = request.Received;

        _applicationDbContext.ShopOrders.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);


        if (!oldReceived && entity.Received)
        {
            var newInventories = entity.ShopOrderProducts.Select(y => new Inventory()
            {
                Description = $"Comanda {entity.Id} Primita",
                MinimumLevel = 10,
                Quantity = y.Quantity,
                ShopProductId = y.ShopProductId,
            }).ToList();
            await _applicationDbContext.Inventories.AddRangeAsync(newInventories, cancellationToken);
        }

        var oldTransaction = await _applicationDbContext.Transactions.Include(x => x.TransactionDetails).FirstOrDefaultAsync(x => x.ShopOrderId == entity.Id, cancellationToken);
        entity = await _applicationDbContext.ShopOrders.Include(x => x.ShopOrderProducts).ThenInclude(x => x.ShopProduct).FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken);

        var newTransaction = new Transaction()
        {
            Description = $"Comanda magazin numarul {entity.Id} produse: " + string.Join(", ", entity.ShopOrderProducts.Select(x => x.Quantity + "X " + x.ShopProduct.Name)),
            ShopOrderId = entity.Id,
            TotalAmount = entity.Cost,
            TransactionDate = entity.OrderDate,
            TransactionTypeId = (long)TransactionTypeEnum.VANZARE,
            TransactionDetails = entity.ShopOrderProducts.Select(x => new TransactionDetail()
            {
                ShopProductId = x.ShopProductId,
                Quantity = x.Quantity,
                Amount = x.Cost,
            }).ToList()
        };

        _applicationDbContext.Transactions.Remove(oldTransaction);

        var coffeeShopBudget = await _applicationDbContext.CoffeeShops.FirstOrDefaultAsync(x => x.Id == _httpContextAccesorService.LocationId, cancellationToken);
        if (coffeeShopBudget != null)
        {
            coffeeShopBudget.Budget -= oldTransaction.TotalAmount;
            coffeeShopBudget.Budget += newTransaction.TotalAmount;
            _applicationDbContext.CoffeeShops.Update(coffeeShopBudget);
        }
        await _applicationDbContext.Transactions.AddAsync(newTransaction, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ShopOrderDto>(entity);
    }
}
