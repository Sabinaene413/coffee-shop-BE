using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;
using MyCoffeeShop.Application.Customers;

namespace MyCoffeeShop.Application.Orders;

public class Order : BaseEntity
{
    public Order()
    {
    }
    public long CustomerId { get; set; }
    public int VatRate { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual List<OrderItem> OrderItems { get; set; }
}

public class OrderDto : IMapFrom<Order>
{
    public long CustomerId { get; set; }
    public long Id { get; set; }
    public int VatRate { get; set; }
    public OrderItemDto OrderItemDto { get; set; }

}
