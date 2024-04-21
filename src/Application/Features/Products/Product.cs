using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;

namespace MyCoffeeShop.Application.Products;

public class Product : BaseEntity
{
    public Product()
    {
    }

    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }

}

public class ProductDto : IMapFrom<Product>
{
    public required long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }

}
