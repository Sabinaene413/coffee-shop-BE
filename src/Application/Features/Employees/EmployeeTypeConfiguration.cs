using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyCoffeeShop.Application.EmployeeTypes;

public class EmployeeTypeConfiguration : IEntityTypeConfiguration<EmployeeType>
{
    public void Configure(EntityTypeBuilder<EmployeeType> builder)
    {
        builder.HasData(new List<EmployeeType>() { new EmployeeType() { Id = (long)EmployeeTypeEnum.BARISTA, Name = EmployeeTypeEnum.BARISTA.ToString() }
            , new EmployeeType() { Id =(long) EmployeeTypeEnum.ASISTENT_MANAGER, Name = EmployeeTypeEnum.ASISTENT_MANAGER.ToString() } });
    }
}
