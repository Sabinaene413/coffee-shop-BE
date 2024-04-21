using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.UIRoutes
{
    public class UIRouteConfiguration : IEntityTypeConfiguration<UIRoute>
    {
        public void Configure(EntityTypeBuilder<UIRoute> builder)
        {
            builder.HasNoDiscriminator();
            builder.Property(p => p.Id);
        }
    }
}
