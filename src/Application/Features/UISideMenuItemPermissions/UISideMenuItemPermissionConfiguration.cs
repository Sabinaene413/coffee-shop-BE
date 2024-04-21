using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace MyCoffeeShop.Application.UISideMenuItemPermissions
{
    public class UISideMenuItemPermissionConfiguration : IEntityTypeConfiguration<UISideMenuItemPermission>
    {
        public void Configure(EntityTypeBuilder<UISideMenuItemPermission> builder)
        {
  
            builder.HasNoDiscriminator();
            builder.Property(p => p.IsHidden);
            builder.Property(p => p.IsDisabled);
            builder.Property(p => p.Active).IsRequired();
        }
    }
}
