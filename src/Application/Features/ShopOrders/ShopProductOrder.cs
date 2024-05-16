using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;
using MyCoffeeShop.Application.ShopProducts;

namespace MyCoffeeShop.Application.ShopOrders;

public class ShopProductOrder : BaseLocationEntity
{
    public ShopProductOrder()
    {
    }

    public long ShopProductId { get; set; }
    public long ShopOrderId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
    public virtual ShopProduct ShopProduct { get; set; }
    public virtual ShopOrder ShopOrder { get; set; }
}

public class ShopProductOrderDto : IMapFrom<ShopProductOrder>
{
    public long? Id { get; set; }
    public long ShopProductId { get; set; }
    public long? ShopOrderId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Cost { get; set; }
}
