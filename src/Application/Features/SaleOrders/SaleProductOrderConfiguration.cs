using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyCoffeeShop.Application.SaleOrders;

public class SaleProductOrderConfiguration : IEntityTypeConfiguration<SaleProductOrder>
{
    public void Configure(EntityTypeBuilder<SaleProductOrder> builder)
    {
        builder.HasOne(x => x.SaleProduct).WithMany(x => x.SaleProductOrders).HasForeignKey(x => x.SaleProductId).OnDelete(DeleteBehavior.NoAction);
    }
}
