using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.UIComponentPermissions;

public record InactiveUIComponentPermissionCommand(long Id) : IRequest;


internal sealed class InactiveUIComponentPermissionCommandHandler : IRequestHandler<InactiveUIComponentPermissionCommand>
{
    private readonly ApplicationDbContext _applicationDbContext;

    public InactiveUIComponentPermissionCommandHandler(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task Handle(InactiveUIComponentPermissionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _applicationDbContext.UIComponentPermissions
                   .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new NotFoundException(nameof(UIComponentPermission), request.Id);

        entity.Active = false;

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

    }

}

