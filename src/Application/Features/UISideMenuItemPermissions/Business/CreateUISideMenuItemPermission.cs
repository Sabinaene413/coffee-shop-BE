using FluentValidation;
using MediatR;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.UISideMenuItemPermissions;

public class CreateUISideMenuItemPermissionCommand : IRequest<long>
{
    public long UISideMenuItemId { get; set; }
    public long BusinessRoleId { get; set; }
    public string? BusinessUnitId { get; set; }
    public bool IsHidden { get; set; } = false;
    public bool IsDisabled { get; set; } = false;
}

public class CreateUISideMenuItemPermissionCommandValidator : AbstractValidator<CreateUISideMenuItemPermissionCommand>
{
    public CreateUISideMenuItemPermissionCommandValidator()
    {

        RuleFor(v => v.UISideMenuItemId)
            .NotEmpty();
        RuleFor(v => v.BusinessRoleId)
           .NotEmpty();
    }
}

internal sealed class CreateUISideMenuItemPermissionCommandHandler : IRequestHandler<CreateUISideMenuItemPermissionCommand, long>
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CreateUISideMenuItemPermissionCommandHandler(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<long> Handle(CreateUISideMenuItemPermissionCommand request, CancellationToken cancellationToken)
    {

        var entityExists = _applicationDbContext.UISideMenuItemPermissions
            .Where(x => x.UISideMenuItemId == request.UISideMenuItemId && x.BusinessRoleId == request.BusinessRoleId && x.Active == true)
            .FirstOrDefault();

        if (entityExists != null)
            throw new NotFoundException("UI Side Menu Item Permission already exists");

        var entity = new UISideMenuItemPermission
        {
            Active = true,
            UISideMenuItemId = request.UISideMenuItemId,
            BusinessRoleId = request.BusinessRoleId,
            IsHidden = request.IsHidden,
            IsDisabled = request.IsDisabled,
            BusinessUnitId = request.BusinessUnitId,
        };

        _applicationDbContext.UISideMenuItemPermissions.Add(entity);

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
