using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;
namespace MyCoffeeShop.Application.EmployeePayments;

public class DeleteEmployeePaymentCommand : IRequest
{
    public long Id { get; set; }
}

internal sealed class DeleteEmployeePaymentCommandHandler : IRequestHandler<DeleteEmployeePaymentCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteEmployeePaymentCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteEmployeePaymentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.EmployeePayments
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new NotFoundException(nameof(EmployeePayment), request.Id);

        _context.EmployeePayments.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

