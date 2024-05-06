using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;
namespace MyCoffeeShop.Application.CoffeeShops;

public class DeleteCoffeeShopCommand : IRequest
{
    public long Id { get; set; }
}

internal sealed class DeleteCoffeeShopCommandHandler : IRequestHandler<DeleteCoffeeShopCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteCoffeeShopCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCoffeeShopCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CoffeeShops
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new NotFoundException(nameof(CoffeeShop), request.Id);

        _context.CoffeeShops.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

