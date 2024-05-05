using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyCoffeeShop.Application.ShopOrders;

public class ShopProductOrderConfiguration : IEntityTypeConfiguration<ShopProductOrder>
{
    public void Configure(EntityTypeBuilder<ShopProductOrder> builder)
    {
        builder.HasOne(x => x.ShopProduct).WithMany(x => x.ShopProductOrders).HasForeignKey(x => x.ShopProductId).OnDelete(DeleteBehavior.NoAction);
    }
}
