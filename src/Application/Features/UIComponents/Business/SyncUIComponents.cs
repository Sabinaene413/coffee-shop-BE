using MediatR;
using FluentValidation;
using MyCoffeeShop.Application.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.UIComponents;

public record SyncUIComponentsCommand(
    SyncUIComponentsRequest[]? NewUIComponents,
    SyncUIComponentsRequest[]? UpdatedUIComponents
) : IRequest<string>;

internal sealed class SyncUIComponentsCommandHandler
    : IRequestHandler<SyncUIComponentsCommand, string>
{
    private readonly ApplicationDbContext _applicationDbContext;

    public SyncUIComponentsCommandHandler(
        ApplicationDbContext applicationDbContext
    )
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<string> Handle(
        SyncUIComponentsCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var newUiComponents = request.NewUIComponents?.Select(newItem => new UIComponent
            {
                UiId = newItem.UiId,
                Component = newItem.Component,
                HasPermissions = newItem.HasPermissions,
                Attrs = newItem.Attrs,
                UiRoute = new UIRoutes.UIRoute()
                {
                    UiId = newItem.UiRoute.UiId,
                    Name = newItem.UiRoute.Name,
                    //Path = newItem.UiRoute.Path,
                    FullPath = newItem.UiRoute.FullPath,
                    ParentUiId = newItem.UiRoute.ParentUiId,
                    //IsLeaf = newItem.UiRoute.IsLeaf
                },
            }).ToList() ?? new List<UIComponent>();

            if (newUiComponents.Any())
                await _applicationDbContext.UIComponents.AddRangeAsync(newUiComponents, cancellationToken);

            if (request.UpdatedUIComponents?.Any() ?? false)
            {
                var uiComponentsDictionary = request.UpdatedUIComponents.ToDictionary(x => x.Id.GetValueOrDefault());
                var uiComponentIds = uiComponentsDictionary.Keys;
                var uiComponentsToUpdate = await _applicationDbContext.UIComponents.Where(uiComponent => uiComponentIds.Contains(uiComponent.Id))
                                            .ToListAsync(cancellationToken);

                uiComponentsToUpdate.ForEach(updatedRoute =>
                {
                    if (uiComponentsDictionary.TryGetValue(updatedRoute.Id, out var valuesToUpdate))
                    {
                        updatedRoute.UiId = valuesToUpdate.UiId;
                        updatedRoute.Component = valuesToUpdate.Component;
                        updatedRoute.HasPermissions = valuesToUpdate.HasPermissions;
                        updatedRoute.Attrs = valuesToUpdate.Attrs;
                        updatedRoute.UiRoute = new UIRoutes.UIRoute()
                        {
                            UiId = valuesToUpdate.UiRoute.UiId,
                            Name = valuesToUpdate.UiRoute.Name,
                            //Path = valuesToUpdate.UiRoute.Path,
                            FullPath = valuesToUpdate.UiRoute.FullPath,
                            ParentUiId = valuesToUpdate.UiRoute.ParentUiId,
                            //IsLeaf = valuesToUpdate.UiRoute.IsLeaf
                        };
                    }
                });
                _applicationDbContext.UIComponents.UpdateRange(uiComponentsToUpdate);
            }

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

        }
        catch (Exception ex)
        {
            throw ex;
        }

        return "Synced";
    }
}
