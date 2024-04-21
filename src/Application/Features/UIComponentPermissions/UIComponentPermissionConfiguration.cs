using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.UIComponentPermissions;

public class UIComponentPermissionConfiguration : IEntityTypeConfiguration<UIComponentPermission>
{
    public void Configure(EntityTypeBuilder<UIComponentPermission> builder)
    {
        builder.HasNoDiscriminator();
        builder.Property(p => p.IsHidden);
        builder.Property(p => p.IsDisabled);
        builder.Property(p => p.Active).IsRequired();
        builder.HasOne(x=> x.UIComponent).WithOne(x=> x.UIComponentPermission).HasForeignKey<UIComponentPermission>(x=> x.UIComponentId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x=> x.UISideMenuItemPermission).WithOne(x=> x.UIComponentPermission).HasForeignKey<UIComponentPermission>(x=> x.UIComponentId).OnDelete(DeleteBehavior.NoAction);
    }
}
