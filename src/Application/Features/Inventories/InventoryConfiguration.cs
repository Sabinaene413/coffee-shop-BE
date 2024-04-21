using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyCoffeeShop.Application.Inventories;

public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.HasOne(x => x.ShopProduct).WithMany(x => x.ProductInventories).HasForeignKey(x => x.ShopProductId);
    }
}
