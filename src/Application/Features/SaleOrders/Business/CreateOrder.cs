using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using MyCoffeeShop.Application.Inventories;

namespace MyCoffeeShop.Application.SaleOrders;

public record CreateOrderCommand(
    decimal Cost,
    DateTime? OrderDate,
    List<SaleProductOrderDto> SaleOrderProducts
) : IRequest<SaleOrderDto>;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(v => v.Cost).NotEmpty();
        RuleFor(v => v.SaleOrderProducts).NotEmpty();
    }
}

internal sealed class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, SaleOrderDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<SaleOrderDto> Handle(
        CreateOrderCommand request,
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

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SaleOrderDto>(entity);
    }
}
