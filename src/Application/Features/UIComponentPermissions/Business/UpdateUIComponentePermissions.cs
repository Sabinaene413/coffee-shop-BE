using MediatR;
using FluentValidation;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.UIComponentPermissions;

public record UpdateUIComponentPermissionCommand : IRequest<long>
{
    public long Id { get; set; }
    public long UIComponentId { get; set; }
    public long UISideMenuItemPermissionId { get; set; }
    public bool IsHidden { get; set; }
    public bool IsDisabled { get; set; }

}

public class UpdateUIComponentPermissionCommandValidator : AbstractValidator<UpdateUIComponentPermissionCommand>
{
    public UpdateUIComponentPermissionCommandValidator()
    {
        RuleFor(v => v.UIComponentId)
            .NotEmpty();
        RuleFor(v => v.UISideMenuItemPermissionId)
           .NotEmpty();
    }
}

internal sealed class UpdateUIComponentPermissionCommandHandler : IRequestHandler<UpdateUIComponentPermissionCommand, long>
{
    private readonly ApplicationDbContext _applicationDbContext;

    public UpdateUIComponentPermissionCommandHandler(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<long> Handle(UpdateUIComponentPermissionCommand request, CancellationToken cancellationToken)
    {

        var entity = await _applicationDbContext.UIComponentPermissions.FirstOrDefaultAsync(x=> x.Id == request.Id , cancellationToken) 
             ?? throw new NotFoundException(nameof(UIComponentPermission), request.Id);


        entity.UIComponentId = request.UIComponentId;
        entity.UISideMenuItemPermissionId = request.UISideMenuItemPermissionId;
        entity.IsDisabled = request.IsDisabled;
        entity.IsHidden = request.IsHidden;

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return entity.Id ;
    }
}

