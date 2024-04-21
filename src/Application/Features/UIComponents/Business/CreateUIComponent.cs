using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Common.Constants;
using MyCoffeeShop.Application.Common.Interfaces;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.UIRoutes;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.UIComponents;

public record CreateUIComponentCommand(
    string UiId,
    string Component,
    bool HasPermissions,
    UIComponentAttrs? Attrs,
    SyncUIRouteDto UiRoute,
    bool Active
) : IRequest<UIComponentDto>;

public class CreateUIComponentCommandValidator : AbstractValidator<CreateUIComponentCommand>
{
    public CreateUIComponentCommandValidator()
    {
        RuleFor(v => v.Component).MaximumLength(ConfigurationConstants.nameStringLength).NotEmpty();
        RuleFor(v => v.UiId).MaximumLength(ConfigurationConstants.nameStringLength).NotEmpty();
        RuleFor(v => v.HasPermissions).NotNull();
        RuleFor(v => v.Active).NotNull();
    }
}

internal sealed class CreateUIComponentCommandHandler
    : IRequestHandler<CreateUIComponentCommand, UIComponentDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateUIComponentCommandHandler(
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
        CreateUIComponentCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new UIComponent
        {
            UiId = request.UiId,
            Component = request.Component,
            HasPermissions = request.HasPermissions,
            Attrs = request.Attrs,
            Active = request.Active,
        };

        if (request.UiRoute != null)
        {
            var routeEntity =
                await _applicationDbContext.UIRoutes.FirstOrDefaultAsync(
                    x=> x.Id == request.UiRoute.Id, cancellationToken
                ) ?? throw new NotFoundException(nameof(UIRoute), request.UiRoute.Id);
            entity.UiRoute = new UIRoute
            {
                Id = routeEntity.Id,
                UiId = routeEntity.UiId,
                FullPath = routeEntity.FullPath
            };
        }

        await _applicationDbContext.UIComponents.AddAsync(entity, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<UIComponentDto>(entity);
    }
}
