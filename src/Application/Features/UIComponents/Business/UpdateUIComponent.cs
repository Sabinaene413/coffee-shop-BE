using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Common.Constants;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Common.Interfaces;
using MyCoffeeShop.Application.UIRoutes;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.UIComponents;

public record UpdateUIComponentCommand(
    long Id,
    string UiId,
    string Component,
    bool HasPermissions,
    UIComponentAttrs? Attrs,
    SyncUIRouteDto UiRoute,
    bool Active
) : IRequest<UIComponentDto>;

public class UpdateUIComponentCommandValidator : AbstractValidator<UpdateUIComponentCommand>
{
    public UpdateUIComponentCommandValidator()
    {
        RuleFor(v => v.Component).MaximumLength(ConfigurationConstants.nameStringLength).NotEmpty();
        RuleFor(v => v.UiId).MaximumLength(ConfigurationConstants.nameStringLength).NotEmpty();
        RuleFor(v => v.HasPermissions).NotNull();
        RuleFor(v => v.Active).NotNull();
    }
}

internal sealed class UpdateUIComponentCommandHandler
    : IRequestHandler<UpdateUIComponentCommand, UIComponentDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public UpdateUIComponentCommandHandler(
        ApplicationDbContext applicationDbContext,
        ICurrentUserService currentUserService,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<UIComponentDto> Handle(
        UpdateUIComponentCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _applicationDbContext.UIComponents
                            .Include(x => x.Attrs)
                            .Include(x => x.UiRoute)
                                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
         ?? throw new NotFoundException(nameof(UIComponent), request.Id);

        entity.Attrs = request.Attrs;
        entity.UiId = request.UiId;
        entity.Component = request.Component;
        entity.HasPermissions = request.HasPermissions;
        entity.Active = request.Active;

        if (request.UiRoute != null)
        {
            var routeEntity =
                await _applicationDbContext.UIRoutes.FirstOrDefaultAsync(
                    x => x.Id == request.UiRoute.Id, cancellationToken
                ) ?? throw new NotFoundException(nameof(UIRoute), request.UiRoute.Id);

            entity.UiRoute = new UIRoute
            {
                Id = routeEntity.Id,
                UiId = routeEntity.UiId,
                FullPath = routeEntity.FullPath
            };
        }
        _applicationDbContext.UIComponents.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UIComponentDto>(entity);
    }
}
