using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;
namespace MyCoffeeShop.Application.ShopProducts;

public class DeleteShopProductCommand : IRequest
{
    public long Id { get; set; }
}

internal sealed class DeleteShopProductCommandHandler : IRequestHandler<DeleteShopProductCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteShopProductCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteShopProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ShopProducts
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new NotFoundException(nameof(ShopProduct), request.Id);

        _context.ShopProducts.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

