using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyCoffeeShop.Application.ShopOrders;

public class ShopOrderConfiguration : IEntityTypeConfiguration<ShopOrder>
{
    public void Configure(EntityTypeBuilder<ShopOrder> builder)
    {
        builder.HasMany(x => x.ShopOrderProducts).WithOne(x => x.ShopOrder).HasForeignKey(x => x.ShopOrderId).OnDelete(DeleteBehavior.NoAction);
    }
}
