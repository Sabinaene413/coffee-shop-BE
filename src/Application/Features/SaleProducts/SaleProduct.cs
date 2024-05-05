using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;

namespace MyCoffeeShop.Application.SaleProducts;

public class SaleProduct : BaseEntity
{
    public SaleProduct()
    {
    }

    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }

}

public class SaleProductDto : IMapFrom<SaleProduct>
{
    public required long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }

}
