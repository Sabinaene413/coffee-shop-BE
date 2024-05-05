using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;
namespace MyCoffeeShop.Application.SaleProducts;

public class DeleteSaleProductCommand : IRequest
{
    public long Id { get; set; }
}

internal sealed class DeleteSaleProductCommandHandler : IRequestHandler<DeleteSaleProductCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteSaleProductCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteSaleProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.SaleProducts
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new NotFoundException(nameof(SaleProduct), request.Id);

        _context.SaleProducts.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

