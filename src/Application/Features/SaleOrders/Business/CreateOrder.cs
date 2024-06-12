using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using MyCoffeeShop.Application.Transactions;
using MyCoffeeShop.Application.TransactionTypes;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Interfaces;

namespace MyCoffeeShop.Application.SaleOrders;

public record CreateSaleOrderCommand(
    decimal Cost,
    DateTime? OrderDate,
    List<SaleProductOrderDto> SaleOrderProducts
) : IRequest<SaleOrderDto>;

public class CreateSaleOrderCommandValidator : AbstractValidator<CreateSaleOrderCommand>
{
    public CreateSaleOrderCommandValidator()
    {
        RuleFor(v => v.Cost).NotEmpty();
        RuleFor(v => v.SaleOrderProducts).NotEmpty();
    }
}

internal sealed class CreateOrderCommandHandler
    : IRequestHandler<CreateSaleOrderCommand, SaleOrderDto>
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

    public async Task<SaleOrderDto> Handle(
        CreateSaleOrderCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new SaleOrder
        {
            Cost = request.Cost,
            OrderDate = request.OrderDate,
            SaleOrderProducts = request.SaleOrderProducts.Select(x => new SaleProductOrder()
            {
                SaleProductId = x.SaleProductId,
                Quantity = x.Quantity,
                Cost = x.Cost,
            }).ToList()
        };

        await _applicationDbContext.SaleOrders.AddAsync(entity, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        entity = await _applicationDbContext.SaleOrders.Include(x => x.SaleOrderProducts).ThenInclude(x=> x.SaleProduct).FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken);
        var newTransaction = new Transaction()
        {
            Description = $"Comanda vanzare numarul {entity.Id} produse: " + string.Join(", ", entity.SaleOrderProducts.Select(x => x.Quantity + "X " + x.SaleProduct.Name)),
            SaleOrderId = entity.Id,
            TotalAmount = entity.Cost,
            TransactionDate = entity.OrderDate,
            TransactionTypeId = (long)TransactionTypeEnum.VANZARE,
            TransactionDetails = entity.SaleOrderProducts.Select(x => new TransactionDetail()
            {
                SaleProductId = x.SaleProductId,
                Quantity = x.Quantity,
                Amount = x.Cost,
            }).ToList()
        };
        await _applicationDbContext.Transactions.AddAsync(newTransaction, cancellationToken);

        var coffeeShopBudget = await _applicationDbContext.CoffeeShops.FirstOrDefaultAsync(x => x.Id == _httpContextAccesorService.LocationId, cancellationToken);
        if (coffeeShopBudget != null)
        {
            coffeeShopBudget.Budget += entity.Cost;
            _applicationDbContext.CoffeeShops.Update(coffeeShopBudget);
        }
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SaleOrderDto>(entity);
    }
}
