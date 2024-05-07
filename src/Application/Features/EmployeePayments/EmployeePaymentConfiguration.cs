using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyCoffeeShop.Application.EmployeePayments;

public class EmployeePaymentConfiguration : IEntityTypeConfiguration<EmployeePayment>
{
    public void Configure(EntityTypeBuilder<EmployeePayment> builder)
    {

    }
}
