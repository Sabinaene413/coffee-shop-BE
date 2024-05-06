using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;
using MyCoffeeShop.Application.ShopProducts;

namespace MyCoffeeShop.Application.Inventories;

public class Inventory : BaseLocationEntity
{
    public Inventory()
    {
    }

    public long ShopProductId { get; set; }
    public long Quantity { get; set; }
    public string Description { get; set; }
    public long? MinimumLevel { get; set; }
    public virtual ShopProduct ShopProduct { get; set; }
}

public class InventoryDto : IMapFrom<Inventory>
{
    public long Id { get; set; }
    public long ShopProductId { get; set; }
    public long Quantity { get; set; }
    public string Description { get; set; }
    public long? MinimumLevel { get; set; }
    public long? LocationId { get; set; }
    public string? LocationName { get; set; }

}
