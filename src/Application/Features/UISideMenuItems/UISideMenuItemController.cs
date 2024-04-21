using Microsoft.AspNetCore.Mvc;
using MyCoffeeShop.Application.Common.Abstracts;

namespace MyCoffeeShop.Application.UISideMenuItems
{
    public class UISideMenuItemController : ApiControllerBase
    {
        [HttpPost("Create")]
        public async Task<ActionResult<UISideMenuItemDto>> Create(
            [FromBody] CreateUISideMenuItemCommand command,
            CancellationToken cancellationToken
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }

        [HttpPut("Update")]
        public async Task<ActionResult<UISideMenuItemDto>> Update(
            [FromBody] UpdateUISideMenuItemCommand command,
            CancellationToken cancellationToken
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }

        [HttpPost("GetById")]
        public async Task<ActionResult<UISideMenuItemDto>> GetById(
            [FromBody] GetUISideMenuItemByIdCommand command,
            CancellationToken cancellationToken
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }

        [HttpPost("Filter")]
        public async Task<List<UISideMenuItemDto>> Filter(
            [FromBody] FilterUISideMenuItemCommand query,
            CancellationToken cancellationToken
        )
        {
            return await Mediator.Send(query, cancellationToken);
        }
    }
}
