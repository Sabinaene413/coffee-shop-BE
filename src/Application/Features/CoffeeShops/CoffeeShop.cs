using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;

namespace MyCoffeeShop.Application.CoffeeShops;

public class CoffeeShop : BaseEntity
{
    public CoffeeShop()
    {
    }

    public string Name { get; set; }
    public decimal Budget { get; set; }
}

public class CoffeeShopDto : IMapFrom<CoffeeShop>
{
    public required long Id { get; set; }
    public string Name { get; set; }
    public decimal Budget { get; set; }
}
