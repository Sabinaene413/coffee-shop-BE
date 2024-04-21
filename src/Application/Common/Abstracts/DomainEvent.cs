namespace MyCoffeeShop.Application.Common.Abstracts;

public interface IHasDomainEvent
{
    public List<DomainEvent> DomainEvents { get; }
}

public abstract class DomainEvent
{
    protected DomainEvent()
    {
        DateOccurred = DateTimeOffset.Now;
    }
    public bool IsPublished { get; set; }
    public DateTimeOffset DateOccurred { get; protected set; } = DateTime.Now;
}
