using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;

namespace MyCoffeeShop.Application.TransactionTypes;

public class TransactionType : BaseEntity
{
    public TransactionType()
    {
    }

    public TransactionType(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

}

public class TransactionTypeDto : IMapFrom<TransactionType>
{
    public  long Id { get; set; }
    public string Name { get; set; }
}


public enum TransactionTypeEnum
{
    IN = 1,
    OUT = 2,
    
}
