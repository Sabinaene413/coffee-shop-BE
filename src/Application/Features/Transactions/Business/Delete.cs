using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;
namespace MyCoffeeShop.Application.Transactions;

public class DeleteTransactionCommand : IRequest
{
    public long Id { get; set; }
}

internal sealed class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteTransactionCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Transactions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Transaction), request.Id);

        _context.Transactions.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

