using MediatR;
using AutoMapper;
using MyCoffeeShop.Application.Common.Interfaces;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.UIRoutes;

public record SyncUIRouteCommand(
    SyncUIRouteRequest[]? NewUiRoutes,
    SyncUIRouteRequest[]? UpdatedUiRoutes
) : IRequest<string>;

internal sealed class SyncUIRouteCommandHandler : IRequestHandler<SyncUIRouteCommand, string>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public SyncUIRouteCommandHandler(
        ApplicationDbContext applicationDbContext,
        ICurrentUserService currentUserService,
        IMapper mapper
    )
    {
        _applicationDbContext = applicationDbContext;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<string> Handle(
        SyncUIRouteCommand request,
        CancellationToken cancellationToken
    )
    {
        var newUiRoutes = request.NewUiRoutes?.Select(newItem => new UIRoute
        {
            Name = newItem.Name,
            UiId = newItem.UiId,
            Path = newItem.Path,
            FullPath = newItem.FullPath,
            ParentUiId = newItem.ParentUiId,
            IsLeaf = newItem.IsLeaf,
        }).ToList() ?? new List<UIRoute>();

        if (newUiRoutes.Any())
            await _applicationDbContext.UIRoutes.AddRangeAsync(newUiRoutes, cancellationToken);

        if (request.UpdatedUiRoutes?.Any() ?? false)
        {
            var uiRoutesDictionary = request.UpdatedUiRoutes.ToDictionary(x => x.Id);
            var routeIds = uiRoutesDictionary.Keys;
            var routesToUpdates = await _applicationDbContext.UIRoutes.Where(uiRoute => routeIds.Contains(uiRoute.Id)).ToListAsync(cancellationToken);

            routesToUpdates.ForEach(updatedRoute =>
            {
                if (uiRoutesDictionary.TryGetValue(updatedRoute.Id, out var valuesToUpdate))
                {
                    updatedRoute.IsLeaf = valuesToUpdate.IsLeaf;
                    updatedRoute.Name = valuesToUpdate.Name;
                    updatedRoute.Path = valuesToUpdate.Path;
                    updatedRoute.FullPath = valuesToUpdate.FullPath;
                    updatedRoute.UiId = valuesToUpdate.UiId;
                    updatedRoute.ParentUiId = valuesToUpdate.ParentUiId;
                }
            });
            _applicationDbContext.UIRoutes.UpdateRange(routesToUpdates);
        }

        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        return "Synced";
    }
}
