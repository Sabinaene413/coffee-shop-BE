using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace MyCoffeeShop.Application.Common.Abstracts;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")] // Specifies the response format for all actions in the controller
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>()!;
}