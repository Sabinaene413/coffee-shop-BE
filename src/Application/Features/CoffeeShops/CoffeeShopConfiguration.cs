using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyCoffeeShop.Application.CoffeeShops;

public class CoffeeShopConfiguration : IEntityTypeConfiguration<CoffeeShop>
{
    public void Configure(EntityTypeBuilder<CoffeeShop> builder)
    {
        builder.Property(c => c.Budget)
            .HasColumnType("decimal(18, 2)");
    }
}
