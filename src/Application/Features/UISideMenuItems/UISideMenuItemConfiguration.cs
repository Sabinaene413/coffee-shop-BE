using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.UISideMenuItems
{
    public class UISideMenuItemConfiguration : IEntityTypeConfiguration<UISideMenuItem>
    {
        public void Configure(EntityTypeBuilder<UISideMenuItem> builder)
        {
            builder.HasNoDiscriminator();

        }
    }
}
