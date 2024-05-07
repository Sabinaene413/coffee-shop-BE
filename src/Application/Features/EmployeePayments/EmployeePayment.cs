using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;
using MyCoffeeShop.Application.Employees;

namespace MyCoffeeShop.Application.EmployeePayments
{
    public class EmployeePayment : BaseLocationEntity
    {
        public EmployeePayment()
        {
        }

        public long? EmployeeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? EmployeePaymentDate { get; set; }

        public virtual Employee Employee { get; set; }
    }

    public class EmployeePaymentDto : IMapFrom<EmployeePayment>
    {
        public long Id { get; set; }
        public long? EmployeeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? EmployeePaymentDate { get; set; }
    }
}
