using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;

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

    public CreateOrderCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
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

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SaleOrderDto>(entity);
    }
}
