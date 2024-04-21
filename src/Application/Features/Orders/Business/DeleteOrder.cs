using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;
namespace MyCoffeeShop.Application.Orders;

public class DeleteOrderCommand : IRequest
{
    public long Id { get; set; }
}

internal sealed class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteOrderCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Orders
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Order), request.Id);

        _context.Orders.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class OrderDeletedEvent : DomainEvent
{
    public OrderDeletedEvent(Order item)
    {
        Item = item;
    }

    public Order Item { get; }
}