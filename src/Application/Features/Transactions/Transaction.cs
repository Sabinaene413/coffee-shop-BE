using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;
using MyCoffeeShop.Application.TransactionTypes;

namespace MyCoffeeShop.Application.Transactions;

public class Transaction : BaseEntity
{
    public Transaction()
    {
    }

    //ShopOrder Invoice, Producer ShopOrder Invoice
    public long? InvoiceId { get; set; }
    public long? ShopProductId { get; set; }
    public long? Quantity { get; set; }
    public decimal Value { get; set; }
    // IN/OUT
    public long TransactionTypeId { get; set; }
    public virtual TransactionType TransactionType { get; set; }

}

public class TransactionDto : IMapFrom<Transaction>
{
    public long Id { get; set; }
    public long ShopProductId { get; set; }
    public long Quantity { get; set; }
    public decimal Value { get; set; }
    public long TransactionTypeId { get; set; }

}
