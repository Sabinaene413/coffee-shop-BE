using MediatR.Pipeline;

using Microsoft.Extensions.Logging;

using MyCoffeeShop.Application.Common.Interfaces;

namespace MyCoffeeShop.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly IHttpContextAccesorService _currentUserService;

    public LoggingBehaviour(ILogger<TRequest> logger, IHttpContextAccesorService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId;

        return Task.Run(() => _logger.LogInformation("Request: {Name} {@UserId} {@Request}",
                requestName, userId, request), cancellationToken);
    }
}