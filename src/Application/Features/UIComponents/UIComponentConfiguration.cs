using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MyCoffeeShop.Application.UIComponents
{
    public class UIComponentConfiguration : IEntityTypeConfiguration<UIComponent>
    {
        public void Configure(EntityTypeBuilder<UIComponent> builder)
        {
            builder.HasNoDiscriminator();
        }
    }
}
