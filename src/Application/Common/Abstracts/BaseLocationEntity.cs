namespace MyCoffeeShop.Application.Common.Abstracts;

public abstract class BaseLocationEntity : BaseEntity
{
    public long? LocationId { get; set; }
    public string? LocationName { get; set; }

}
