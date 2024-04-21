using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;

namespace MyCoffeeShop.Application.UISideMenuItemPermissions;

public record UpdateUISideMenuItemPermissionCommand : IRequest<long>
{
    public long Id { get; set; }
    public long UISideMenuItemId { get; set; }
    public long BusinessRoleId { get; set; }
    public string? BusinessUnitId { get; set; }
    public bool IsHidden { get; set; } = false;
    public bool IsDisabled { get; set; } = false;
}

public class UpdateUISideMenuItemPermissionCommandValidator : AbstractValidator<UpdateUISideMenuItemPermissionCommand>
{
    public UpdateUISideMenuItemPermissionCommandValidator()
    {
        RuleFor(v => v.Id)
              .NotEmpty();
        RuleFor(v => v.UISideMenuItemId)
            .NotEmpty();
        RuleFor(v => v.BusinessRoleId)
           .NotEmpty();

    }
}

internal sealed class UpdateUISideMenuItemPermissionCommandHandler : IRequestHandler<UpdateUISideMenuItemPermissionCommand, long>
{
    private readonly ApplicationDbContext _applicationDbContext;

    public UpdateUISideMenuItemPermissionCommandHandler(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<long> Handle(UpdateUISideMenuItemPermissionCommand request, CancellationToken cancellationToken)
    {

        var entity = await _applicationDbContext.UISideMenuItemPermissions.FirstOrDefaultAsync(x=> x.Id == request.Id, cancellationToken);


        if (entity is null)
            throw new NotFoundException(nameof(UISideMenuItemPermission), request.Id);

        entity.UISideMenuItemId = request.UISideMenuItemId;
        entity.BusinessRoleId = request.BusinessRoleId;
        entity.IsHidden = request.IsHidden;
        entity.IsDisabled = request.IsDisabled;
        entity.BusinessUnitId = request.BusinessUnitId;

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}

