using MediatR;

using Microsoft.Extensions.Logging;
using MyCoffeeShop.Application.Common.Models;
namespace MyCoffeeShop.Application.Users;

public class UserCompletedEventHandler : INotificationHandler<DomainEventNotification<UserCompletedEvent>>
{
    private readonly ILogger<UserCompletedEventHandler> _logger;

    public UserCompletedEventHandler(ILogger<UserCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<UserCompletedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

        return Task.CompletedTask;
    }
}
