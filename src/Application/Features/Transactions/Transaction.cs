using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;
using MyCoffeeShop.Application.TransactionTypes;

namespace MyCoffeeShop.Application.Transactions;

public class Transaction : BaseLocationEntity
{
    public Transaction()
    {
    }

    public string Description { get; set; }
    public long? EmployeePaymentId { get; set; }
    public long? ShopOrderId { get; set; }
    public long? SaleOrderId { get; set; }
    public decimal TotalAmount { get; set; }
    // IN/OUT
    public long TransactionTypeId { get; set; }
    public DateTime? TransactionDate { get; set; }
    public virtual TransactionType TransactionType { get; set; }
    public virtual List<TransactionDetail> TransactionDetails { get; set; }

}

public class TransactionDto : IMapFrom<Transaction>
{
    public long Id { get; set; }
    public string Description { get; set; }
    public long? EmployeePaymentId { get; set; }
    public long? ShopOrderId { get; set; }
    public long? SaleOrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public long TransactionTypeId { get; set; }
    public DateTime? TransactionDate { get; set; }

}
