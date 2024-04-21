using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;

namespace MyCoffeeShop.Application.UIRoutes;

public class UIRoute : BaseEntity
{
    public UIRoute()
    {
    }

    public string UiId { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string FullPath { get; set; }
    public string? ParentUiId { get; set; }
    public bool IsLeaf { get; set; } = false;
}

public class UIRouteDto : IMapFrom<UIRoute>
{
    public long Id { get; set; }
    public string UiId { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string FullPath { get; set; }
    public string? ParentUiId { get; set; }
    public bool IsLeaf { get; set; } = false;
}

public class SyncUIRouteDto : IMapFrom<UIRoute>
{
    public long Id { get; set; }
    public string UiId { get; set; }
    public string Name { get; set; }
    public string FullPath { get; set; }
    public string? ParentUiId { get; set; }
}

public record SyncUIRouteRequest
{
    public long Id { get; set; }
    public string UiId { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string FullPath { get; set; }
    public string? ParentUiId { get; set; }
    public bool IsLeaf { get; set; } = false;
}
