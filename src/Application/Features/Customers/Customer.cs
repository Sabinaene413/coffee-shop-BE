using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;

namespace MyCoffeeShop.Application.Customers;

public class Customer : BaseEntity
{
    public Customer()
    {
    }

    public string Name { get; set; }
    public string Address { get; set; }
    public string Telephone { get; set; }
    public string Email { get; set; }

}

public class CustomerDto : IMapFrom<Customer>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Telephone { get; set; }
    public string Email { get; set; }

}
