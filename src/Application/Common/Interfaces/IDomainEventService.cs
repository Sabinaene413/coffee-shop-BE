using MyCoffeeShop.Application.Common.Abstracts;

namespace MyCoffeeShop.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task Publish(DomainEvent domainEvent);
}
