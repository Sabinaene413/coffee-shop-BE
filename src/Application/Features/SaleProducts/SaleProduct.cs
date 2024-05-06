using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;
using MyCoffeeShop.Application.SaleOrders;

namespace MyCoffeeShop.Application.SaleProducts;

public class SaleProduct : BaseLocationEntity
{
    public SaleProduct()
    {
    }

    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public List<SaleProductOrder> SaleProductOrders { get; set; }

}

public class SaleProductDto : IMapFrom<SaleProduct>
{
    public required long Id { get; set; }
    public long? LocationId { get; set; }
    public string? LocationName { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }

}
