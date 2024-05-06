using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;
using MyCoffeeShop.Application.Inventories;
using MyCoffeeShop.Application.ShopOrders;

namespace MyCoffeeShop.Application.ShopProducts;

public class ShopProduct : BaseLocationEntity
{
    public ShopProduct()
    {
    }

    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public List<ShopProductOrder> ShopProductOrders { get; set; }
    public List<Inventory> ProductInventories { get; set; }

}

public class ShopProductDto : IMapFrom<ShopProduct>
{
    public required long Id { get; set; }
    public long? LocationId { get; set; }
    public string? LocationName { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }

}
