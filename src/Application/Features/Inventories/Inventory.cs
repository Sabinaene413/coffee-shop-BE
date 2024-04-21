using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;
using MyCoffeeShop.Application.Products;

namespace MyCoffeeShop.Application.Inventories;

public class Inventory : BaseEntity
{
    public Inventory()
    {
    }

    public long ProductId { get; set; }
    public long Quantity { get; set; }
    public string Description { get; set; }
    public long? MinimumLevel { get; set; }
    public DateTime ExpiryDate { get; set; }
    public virtual Product Product { get; set; }
}

public class InventoryDto : IMapFrom<Inventory>
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public long Quantity { get; set; }
    public string Description { get; set; }
    public long? MinimumLevel { get; set; }
    public DateTime ExpiryDate { get; set; }

}
