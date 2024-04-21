using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.UIComponentPermissions;
using MyCoffeeShop.Application.UISideMenuItems;

namespace MyCoffeeShop.Application.UISideMenuItemPermissions
{
    public class UISideMenuItemPermission : BaseEntity
    {
        public UISideMenuItemPermission()
        {
        }

        public long UISideMenuItemId { get; set; }
        public long BusinessRoleId { get; set; }
        public string? BusinessUnitId { get; set; }
        public bool IsHidden { get; set; } = false;
        public bool IsDisabled { get; set; } = false;
        public virtual UISideMenuItem? UISideMenuItem { get; set; }
        public virtual UIComponentPermission UIComponentPermission { get; set; }

    }
}
