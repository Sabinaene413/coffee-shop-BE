using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;
using MyCoffeeShop.Application.Orders;

namespace MyCoffeeShop.Application.Invoices;

public class Invoice : BaseEntity
{
    public Invoice()
    {
    }

    public long OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public long PaymentStatus { get; set; }

    public virtual Order Order { get; set; }

}

public class InvoiceDto : IMapFrom<Invoice>
{
    public required long Id { get; set; }
    public long OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public long PaymentStatus { get; set; }


}
