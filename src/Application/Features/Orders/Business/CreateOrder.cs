using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.Orders;

public record CreateOrderCommand(
    long CustomerId,
    int VatRate,
    List<OrderItemDto> OrderItems
) : IRequest<OrderDto>;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(v => v.CustomerId).NotEmpty();
        RuleFor(v => v.OrderItems).NotEmpty();
        RuleFor(v => v.VatRate).NotEmpty();
    }
}

internal sealed class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, OrderDto>
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

    public async Task<OrderDto> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new Order
        {
            CustomerId = request.CustomerId,
            VatRate = request.VatRate,
            OrderItems = request.OrderItems.Select(x=> new OrderItem()
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
            }).ToList()
        };

        await _applicationDbContext.Orders.AddAsync(entity, cancellationToken);
        return _mapper.Map<OrderDto>(entity);
    }
}
