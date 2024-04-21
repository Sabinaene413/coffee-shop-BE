using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;

namespace MyCoffeeShop.Application.Orders;

public record UpdateOrderCommand(
    long Id,
    long CustomerId,
    int VatRate,
    List<OrderItemDto> OrderItems,
    bool Active
) : IRequest<OrderDto>;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(v => v.CustomerId).NotEmpty();
        RuleFor(v => v.OrderItems).NotEmpty();
        RuleFor(v => v.VatRate).NotEmpty();
    }
}

internal sealed class UpdateOrderCommandHandler
    : IRequestHandler<UpdateOrderCommand, OrderDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper _mapper;

    public UpdateOrderCommandHandler(
        ApplicationDbContext applicationDbContext,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _mapper = mapper;
    }

    public async Task<OrderDto> Handle(
        UpdateOrderCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.Orders.Include(x => x.OrderItems)
                                      .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
               ?? throw new NotFoundException(nameof(Order), request.Id);

        entity.OrderItems = request.OrderItems.Select(x => new OrderItem()
        {
            ProductId = x.ProductId,
            Quantity = x.Quantity,
            UnitPrice = x.UnitPrice,
        }).ToList();

        entity.CustomerId = request.CustomerId;
        entity.VatRate = request.VatRate;
        entity.Active = request.Active;

        _applicationDbContext.Orders.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<OrderDto>(entity);
    }
}
