using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;

namespace MyCoffeeShop.Application.Orders;

public class OrderItem : BaseEntity
{
    public OrderItem()
    {
    }

    public long ProductId { get; set; }
    public long OrderId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public virtual Order Order { get; set; }
}

public class OrderItemDto : IMapFrom<OrderItem>
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public long OrderId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
