using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;
namespace MyCoffeeShop.Application.Products;

public class DeleteProductCommand : IRequest
{
    public long Id { get; set; }
}

internal sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteProductCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Product), request.Id);

        _context.Products.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public class ProductDeletedEvent : DomainEvent
{
    public ProductDeletedEvent(Product item)
    {
        Item = item;
    }

    public Product Item { get; }
}