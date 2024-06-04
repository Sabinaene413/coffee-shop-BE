using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using MyCoffeeShop.Application.Inventories;
using MyCoffeeShop.Application.Transactions;
using MyCoffeeShop.Application.TransactionTypes;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Interfaces;

namespace MyCoffeeShop.Application.ShopOrders;

public record CreateOrderCommand(
    string Supplier,
    decimal Cost,
    DateTime? OrderDate,
    DateTime? ArrivalDate,
    bool Received,
    List<ShopProductOrderDto> ShopOrderProducts
) : IRequest<ShopOrderDto>;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(v => v.Supplier).NotEmpty();
    }
}

internal sealed class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, ShopOrderDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccesorService _httpContextAccesorService;

    public CreateOrderCommandHandler(
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
        CreateOrderCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new ShopOrder
        {
            Supplier = request.Supplier,
            Cost = request.Cost,
            OrderDate = request.OrderDate,
            ArrivalDate = request.ArrivalDate,
            Received = request.Received,
            ShopOrderProducts = request.ShopOrderProducts?.Select(x => new ShopProductOrder()
            {
                ShopProductId = x.ShopProductId,
                Quantity = x.Quantity,
                Price = x.Price,
                Cost = x.Cost,
            }).ToList() ?? new List<ShopProductOrder> { }
        };


        await _applicationDbContext.ShopOrders.AddAsync(entity, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        if (entity.Received)
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

        entity = await _applicationDbContext.ShopOrders.Include(x => x.ShopOrderProducts).ThenInclude(x=> x.ShopProduct).FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken);
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
        var coffeeShopBudget = await _applicationDbContext.CoffeeShops.FirstOrDefaultAsync(x => x.Id == _httpContextAccesorService.LocationId, cancellationToken);
        if (coffeeShopBudget != null)
        {
            coffeeShopBudget.Budget += newTransaction.TotalAmount;
            _applicationDbContext.CoffeeShops.Update(coffeeShopBudget);
        }
        await _applicationDbContext.Transactions.AddAsync(newTransaction, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);


        return _mapper.Map<ShopOrderDto>(entity);
    }
}
