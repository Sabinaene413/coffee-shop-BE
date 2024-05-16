using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace MyCoffeeShop.Application.Common.Abstracts;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")] // Specifies the response format for all actions in the controller
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>()!;
}