using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;
namespace MyCoffeeShop.Application.Inventories;

public class DeleteInventoryCommand : IRequest
{
    public long Id { get; set; }
}

internal sealed class DeleteInventoryCommandHandler : IRequestHandler<DeleteInventoryCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteInventoryCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteInventoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Inventories
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Inventory), request.Id);

        _context.Inventories.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
