using FluentValidation;
using MediatR;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.UIComponentPermissions;

public class CreateUIComponentPermissionCommand : IRequest<long>
{
    public long UIComponentId { get; set; }
    public long UISideMenuItemPermissionId { get; set; }
    public bool IsHidden { get; set; }
    public bool IsDisabled { get; set; }
}

public class CreateUIComponentPermissionCommandValidator : AbstractValidator<CreateUIComponentPermissionCommand>
{
    public CreateUIComponentPermissionCommandValidator()
    {
        RuleFor(v => v.UIComponentId)
            .NotEmpty();
        RuleFor(v => v.UISideMenuItemPermissionId)
           .NotEmpty();
    }
}

internal sealed class CreateUIComponentPermissionsCommandHandler : IRequestHandler<CreateUIComponentPermissionCommand, long>
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CreateUIComponentPermissionsCommandHandler(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<long> Handle(CreateUIComponentPermissionCommand request, CancellationToken cancellationToken)
    {

        var entityExists = _applicationDbContext.UIComponentPermissions
            .Where(x => x.UIComponentId == request.UIComponentId && x.UISideMenuItemPermissionId == request.UISideMenuItemPermissionId && x.Active == true)
            .FirstOrDefault();

        if (entityExists != null)
            throw new NotFoundException("UI Component Permission already exists");

        var entity = new UIComponentPermission
        {
            Active = true,
            UIComponentId = request.UIComponentId,
            IsDisabled = request.IsDisabled,
            IsHidden = request.IsHidden,
            UISideMenuItemPermissionId = request.UISideMenuItemPermissionId,
        };

        _applicationDbContext.UIComponentPermissions.Add(entity);

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
