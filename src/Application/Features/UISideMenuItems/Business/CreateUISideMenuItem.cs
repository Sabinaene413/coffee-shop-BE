using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Common.Constants;
using MyCoffeeShop.Application.Common.Interfaces;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.UIRoutes;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.UISideMenuItems;

public record CreateUISideMenuItemCommand(
    string Label,
    string Icon,
    int Order,
    bool Active,
    SyncUIRouteDto UiRoute,
    long? ParentId
) : IRequest<UISideMenuItemDto>;

public class CreateUISideMenuItemCommandValidator : AbstractValidator<CreateUISideMenuItemCommand>
{
    public CreateUISideMenuItemCommandValidator()
    {
        RuleFor(v => v.Label).MaximumLength(ConfigurationConstants.nameStringLength).NotNull();
        RuleFor(v => v.Icon).MaximumLength(ConfigurationConstants.nameStringLength).NotNull();
        RuleFor(v => v.Order).NotNull();
        RuleFor(v => v.Active).NotNull();
    }
}

internal sealed class CreateUISideMenuItemCommandHandler
    : IRequestHandler<CreateUISideMenuItemCommand, UISideMenuItemDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CreateUISideMenuItemCommandHandler(
        ApplicationDbContext applicationDbContext,
        ICurrentUserService currentUserService,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<UISideMenuItemDto> Handle(
        CreateUISideMenuItemCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = new UISideMenuItem
        {
            Label = request.Label,
            Icon = request.Icon,
            Order = request.Order,
            ParentId = request.ParentId,
            Active = request.Active,
        };

        if (request.UiRoute != null)
        {
            var routeEntity = await _applicationDbContext.UIRoutes.Where(x => x.Id == request.UiRoute.Id).FirstOrDefaultAsync(cancellationToken)
                        ?? throw new NotFoundException(nameof(UIRoute), request.UiRoute.Id);
            entity.UiRoute = new UIRoute
            {
                Id = routeEntity.Id,
                UiId = routeEntity.UiId,
                FullPath = routeEntity.FullPath
            };
        }

        await _applicationDbContext.UISideMenuItems.AddAsync(entity, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<UISideMenuItemDto>(entity);
    }
}
