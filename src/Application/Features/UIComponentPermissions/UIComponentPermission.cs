using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.UIComponents;
using MyCoffeeShop.Application.UISideMenuItemPermissions;

namespace MyCoffeeShop.Application.UIComponentPermissions;

public class UIComponentPermission : BaseEntity
{
    public UIComponentPermission()
    {
    }

    public long UIComponentId { get; set; }
    public long UISideMenuItemPermissionId { get; set; }
    public bool IsHidden { get; set; } = false;
    public bool IsDisabled { get; set; } = false;
    public virtual UIComponent? UIComponent { get; set; }
    public virtual UISideMenuItemPermission? UISideMenuItemPermission { get; set; }

}
