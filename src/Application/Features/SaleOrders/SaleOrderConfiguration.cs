using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyCoffeeShop.Application.SaleOrders;

public class SaleOrderConfiguration : IEntityTypeConfiguration<SaleOrder>
{
    public void Configure(EntityTypeBuilder<SaleOrder> builder)
    {
        builder.HasMany(x => x.SaleOrderProducts).WithOne(x => x.SaleOrder).HasForeignKey(x => x.SaleOrderId).OnDelete(DeleteBehavior.NoAction);
    }
}
