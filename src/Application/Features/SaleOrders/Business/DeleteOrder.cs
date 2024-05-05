using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;
namespace MyCoffeeShop.Application.SaleOrders;

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
        var entity = await _context.SaleOrders
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new NotFoundException(nameof(SaleOrder), request.Id);

        _context.SaleOrders.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

