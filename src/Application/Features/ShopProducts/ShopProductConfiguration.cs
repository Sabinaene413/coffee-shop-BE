using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyCoffeeShop.Application.ShopProducts;

public class ShopProductConfiguration : IEntityTypeConfiguration<ShopProduct>
{
    public void Configure(EntityTypeBuilder<ShopProduct> builder)
    {

    }
}
