using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;

namespace MyCoffeeShop.Application.Transactions;

public class TransactionDetail : BaseLocationEntity
{
    public TransactionDetail()
    {
    }
    public long TransactionId { get; set; }
    public long? ShopProductId { get; set; }
    public long? SaleProductId { get; set; }
    public long? Quantity { get; set; }
    public decimal Amount { get; set; }
    public virtual Transaction Transaction { get; set; }
}

public class TransactionDetailDto : IMapFrom<TransactionDetail>
{
    public long Id { get; set; }
    public long? ShopProductId { get; set; }
    public long? SaleProductId { get; set; }
    public long? Quantity { get; set; }
    public decimal Amount { get; set; }

}
