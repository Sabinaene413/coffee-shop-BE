using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;
namespace MyCoffeeShop.Application.ShopOrders;

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
        var entity = await _context.ShopOrders
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new NotFoundException(nameof(ShopOrder), request.Id);

        _context.ShopOrders.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class OrderDeletedEvent : DomainEvent
{
    public OrderDeletedEvent(ShopOrder item)
    {
        Item = item;
    }

    public ShopOrder Item { get; }
}