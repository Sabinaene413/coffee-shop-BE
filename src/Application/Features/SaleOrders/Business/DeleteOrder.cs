using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;
namespace MyCoffeeShop.Application.SaleOrders;

public class DeleteSaleOrderCommand : IRequest
{
    public long Id { get; set; }
}

internal sealed class DeleteSaleOrderCommandHandler : IRequestHandler<DeleteSaleOrderCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteSaleOrderCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSaleOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.SaleOrders
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new NotFoundException(nameof(SaleOrder), request.Id);

        _context.SaleOrders.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

