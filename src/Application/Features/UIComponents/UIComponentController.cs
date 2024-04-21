using Microsoft.AspNetCore.Mvc;
using MyCoffeeShop.Application.Common.Abstracts;

namespace MyCoffeeShop.Application.UIComponents
{
    public class UIComponentController : ApiControllerBase
    {
        [HttpPost("Create")]
        public async Task<ActionResult<UIComponentDto>> Create(
            CreateUIComponentCommand command,
            CancellationToken cancellationToken
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }

        [HttpPut("Update")]
        public async Task<ActionResult<UIComponentDto>> Update(
            [FromBody] UpdateUIComponentCommand command,
            CancellationToken cancellationToken
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }

        [HttpPost("GetById")]
        public async Task<ActionResult<UIComponentDto>> GetById(
            [FromBody] GetUIComponentByIdCommand command,
            CancellationToken cancellationToken
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }

        [HttpPost("Filter")]
        public async Task<List<UIComponentDto>> Filter(
            [FromBody] FilterUIComponentsCommand query,
            CancellationToken cancellationToken
        )
        {
            return await Mediator.Send(query, cancellationToken);
        }

        [HttpGet("GetByRouteId/{id}")]
        public async Task<ActionResult<List<UIComponentDto>>> GetByRouteId(
            long id,
            CancellationToken cancellationToken
        )
        {
            return Ok(
                await Mediator.Send(new GetUIComponentByRouteIdCommand(id), cancellationToken)
            );
        }

        [HttpPost("Sync")]
        public async Task<IActionResult> Sync(
            [FromBody] SyncUIComponentsCommand items,
            CancellationToken cancellationToken
        )
        {
            return Ok(await Mediator.Send(items, cancellationToken));
        }
    }
}
