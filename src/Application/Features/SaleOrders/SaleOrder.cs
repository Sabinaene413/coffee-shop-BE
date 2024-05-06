using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;

namespace MyCoffeeShop.Application.SaleOrders;

public class SaleOrder : BaseLocationEntity
{
    public SaleOrder()
    {
    }
    public decimal Cost { get; set; }
    public DateTime? OrderDate { get; set; }
    public virtual List<SaleProductOrder> SaleOrderProducts { get; set; }
}

public class SaleOrderDto : IMapFrom<SaleOrder>
{
    public long Id { get; set; }
    public long? LocationId { get; set; }
    public string? LocationName { get; set; }
    public decimal Cost { get; set; }
    public DateTime? OrderDate { get; set; }

    public List<SaleProductOrderDto> SaleOrderProducts { get; set; }

}
