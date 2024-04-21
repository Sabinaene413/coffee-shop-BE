using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;
using MyCoffeeShop.Application.UIRoutes;

namespace MyCoffeeShop.Application.UISideMenuItems;
public class UISideMenuItem : BaseEntity
{
    public long? ParentId { get; set; }
    public required string Label { get; set; }
    public required string Icon { get; set; }
    public int Order { get; set; }
    public UIRoute UiRoute { get; set; }
}

public class UISideMenuItemDto : IMapFrom<UISideMenuItem>
{
    public long Id { get; set; }
    public long? ParentId { get; set; }
    public string? Label { get; set; }
    public string? Icon { get; set; }
    public int? Order { get; set; }
    public SyncUIRouteDto UiRoute { get; set; }
    public bool Active { get; set; }
}