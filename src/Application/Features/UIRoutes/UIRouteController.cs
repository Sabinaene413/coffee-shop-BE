using Microsoft.AspNetCore.Mvc;
using MyCoffeeShop.Application.Common.Abstracts;

namespace MyCoffeeShop.Application.UIRoutes
{
    public class UIRouteController : ApiControllerBase
    {
        [HttpPost("Create")]
        public async Task<ActionResult<UIRouteDto>> Create(
            [FromBody] CreateUIRouteCommand command,
            CancellationToken cancellationToken
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }

        [HttpPut("Update")]
        public async Task<ActionResult<UIRouteDto>> Update(
            [FromBody] UpdateUIRouteCommand command,
            CancellationToken cancellationToken
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }

        [HttpPost("GetById")]
        public async Task<ActionResult<UIRouteDto>> GetById(
            [FromBody] GetUIRouteByIdCommand command,
            CancellationToken cancellationToken
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }

        [HttpPost("Filter")]
        public async Task<ActionResult<List<UIRouteDto>>> Filter(
            [FromBody] FilterUIRoutesCommand query,
            CancellationToken cancellationToken
        )
        {
            return Ok(await Mediator.Send(query, cancellationToken));
        }

        [HttpPost("All")]
        public async Task<ActionResult<List<SyncUIRouteDto>>> All(
            [FromBody] AllUIRoutesCommand query,
            CancellationToken cancellationToken
        )
        {
            return Ok(await Mediator.Send(query, cancellationToken));
        }

        [HttpPost("Sync")]
        public async Task<ActionResult<string>> Sync(
            [FromBody] SyncUIRouteCommand items,
            CancellationToken cancellationToken
        )
        {
            return Ok(await Mediator.Send(items, cancellationToken));
        }
    }
}
