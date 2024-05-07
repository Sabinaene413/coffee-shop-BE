using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyCoffeeShop.Application.Employees;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasMany(x => x.EmployeePayments).WithOne(x => x.Employee).HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.NoAction);

    }
}
