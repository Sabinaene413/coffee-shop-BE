using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;
namespace MyCoffeeShop.Application.Invoices;

public class DeleteInvoiceCommand : IRequest
{
    public long Id { get; set; }
}

internal sealed class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteInvoiceCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Invoices
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Invoice), request.Id);

        _context.Invoices.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class InvoiceDeletedEvent : DomainEvent
{
    public InvoiceDeletedEvent(Invoice item)
    {
        Item = item;
    }

    public Invoice Item { get; }
}