using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;
using MyCoffeeShop.Application.SaleProducts;

namespace MyCoffeeShop.Application.SaleOrders;

public class SaleProductOrder : BaseLocationEntity
{
    public SaleProductOrder()
    {
    }

    public long SaleProductId { get; set; }
    public long SaleOrderId { get; set; }
    public int Quantity { get; set; }
    public decimal Cost { get; set; }
    public virtual SaleProduct SaleProduct { get; set; }
    public virtual SaleOrder SaleOrder { get; set; }
}

public class SaleProductOrderDto : IMapFrom<SaleProductOrder>
{
    public long Id { get; set; }
    public long SaleProductId { get; set; }
    public long SaleOrderId { get; set; }
    public int Quantity { get; set; }
    public decimal Cost { get; set; }
}
