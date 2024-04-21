using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;

namespace MyCoffeeShop.Application.Employees;

public class Employee : BaseEntity
{
    public Employee()
    {
    }

    public string Name { get; set; }
    public string Position { get; set; }
    public string ContactInformation { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime BirthDate  { get; set; }
    public string Address { get; set; }

}

public class EmployeeDto : IMapFrom<Employee>
{
    public required long Id { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }
    public string ContactInformation { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime BirthDate { get; set; }
    public string Address { get; set; }

}
