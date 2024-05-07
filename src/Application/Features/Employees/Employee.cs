using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;
using MyCoffeeShop.Application.EmployeePayments;

namespace MyCoffeeShop.Application.Employees;

public class Employee : BaseLocationEntity
{
    public Employee()
    {
    }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FilePath { get; set; }
    public decimal? SalaryNet { get; set; }
    public decimal? SalaryBrut { get; set; }
    public decimal? Taxes { get; set; }
    public virtual List<EmployeePayment> EmployeePayments { get; set; }

}

public class EmployeeDto : IMapFrom<Employee>
{
    public required long Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FilePath { get; set; }
    public decimal? SalaryNet { get; set; }
    public decimal? SalaryBrut { get; set; }
    public decimal? Taxes { get; set; }
    public long? LocationId { get; set; }
    public string? LocationName { get; set; }
}
