using MediatR;
using AutoMapper;
using FluentValidation;
using MyCoffeeShop.Application.Common.Constants;
using MyCoffeeShop.Application.Common.Exceptions;
using MyCoffeeShop.Application.Common.Interfaces;
using MyCoffeeShop.Application.UIRoutes;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.UISideMenuItems;

public record UpdateUISideMenuItemCommand(
    long Id,
    string Label,
    string Icon,
    int Order,
    bool Active,
    long? ParentId,
    SyncUIRouteDto? UiRoute
) : IRequest<UISideMenuItemDto>;

public class UpdateUISideMenuItemCommandValidator : AbstractValidator<UpdateUISideMenuItemCommand>
{
    public UpdateUISideMenuItemCommandValidator()
    {
        RuleFor(v => v.Label).MaximumLength(ConfigurationConstants.nameStringLength).NotNull();
        RuleFor(v => v.Icon).MaximumLength(ConfigurationConstants.nameStringLength).NotNull();
        RuleFor(v => v.Order).NotNull();
        RuleFor(v => v.Active).NotNull();
    }
}

internal sealed class UpdateUISideMenuItemCommandHandler
    : IRequestHandler<UpdateUISideMenuItemCommand, UISideMenuItemDto>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public UpdateUISideMenuItemCommandHandler(
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
        UpdateUISideMenuItemCommand request,
        CancellationToken cancellationToken
    )
    {

        var entity = await _applicationDbContext.UISideMenuItems.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(UISideMenuItem), request.Id);

        entity.Label = request.Label;
        entity.Order = request.Order;
        entity.Icon = request.Icon;
        entity.ParentId = request.ParentId;
        entity.Active = request.Active;

        if (request.UiRoute != null)
        {
            var routeEntity =
                await _applicationDbContext.UIRoutes.FirstOrDefaultAsync(
                    x => x.Id == request.UiRoute.Id,
                    cancellationToken
                ) ?? throw new NotFoundException(nameof(UIRoute), request.UiRoute.Id);
            entity.UiRoute =
                    new UIRoute
                    {
                        Id = routeEntity.Id,
                        UiId = routeEntity.UiId,
                        FullPath = routeEntity.FullPath
                    };
        }

        _applicationDbContext.UISideMenuItems.Update(entity);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UISideMenuItemDto>(entity);
    }
}
