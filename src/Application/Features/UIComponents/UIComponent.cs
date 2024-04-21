using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;
using MyCoffeeShop.Application.UIComponentPermissions;
using MyCoffeeShop.Application.UIRoutes;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCoffeeShop.Application.UIComponents;

public class UIComponent : BaseEntity
{
    public UIComponent()
    {
    }

    public required string UiId { get; set; }
    public required string Component { get; set; }
    public bool HasPermissions { get; set; }
    [NotMapped]
    public UIComponentAttrs? Attrs { get; set; }
    public UIRoute UiRoute { get; set; }
    public UIComponentPermission UIComponentPermission { get; set; }
}

public class UIComponentAttrs
{
    public Dictionary<string, string>? Label { get; set; }
    public Dictionary<string, string>? Placeholder { get; set; }
}

public class UIComponentDto : IMapFrom<UIComponent>
{
    public required long Id { get; set; }
    public required string UiId { get; set; }
    public required string Component { get; set; }
    public bool HasPermissions { get; set; }
    public UIComponentAttrs? Attrs { get; set; }
    public required SyncUIRouteDto UiRoute { get; set; }
    public bool Active { get; set; }
}

public record SyncUIComponentsRequest
{
    public long? Id { get; set; }
    public string UiId { get; set; }
    public string Component { get; set; }
    public bool HasPermissions { get; set; }
    public UIComponentAttrs? Attrs { get; set; }
    public SyncUIRouteDto UiRoute { get; set; }
}
