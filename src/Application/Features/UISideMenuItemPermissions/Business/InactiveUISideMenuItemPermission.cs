using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.UISideMenuItemPermissions;

public record InactiveUISideMenuItemPermissionCommand(long Id) : IRequest;


internal sealed class InactiveUISideMenuItemPermissionCommandHandler : IRequestHandler<InactiveUISideMenuItemPermissionCommand>
{
    private readonly ApplicationDbContext _applicationDbContext;

    public InactiveUISideMenuItemPermissionCommandHandler(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task Handle(InactiveUISideMenuItemPermissionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _applicationDbContext.UISideMenuItemPermissions
                   .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(UISideMenuItemPermission), request.Id);
        }

        entity.Active = false;

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

    }

}

