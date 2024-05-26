using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;

namespace MyCoffeeShop.Application.EmployeeTypes;

public class EmployeeType : BaseEntity
{
    public EmployeeType()
    {
    }

    public EmployeeType(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

}

public class EmployeeTypeDto : IMapFrom<EmployeeType>
{
    public  long Id { get; set; }
    public string Name { get; set; }
}


public enum EmployeeTypeEnum
{
    BARISTA = 1,
    ASISTENT_MANAGER = 2,
    
}
