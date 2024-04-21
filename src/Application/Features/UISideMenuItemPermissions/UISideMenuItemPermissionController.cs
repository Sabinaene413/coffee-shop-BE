using Microsoft.AspNetCore.Mvc;
using MyCoffeeShop.Application.Common.Abstracts;

namespace MyCoffeeShop.Application.UISideMenuItemPermissions
{
    public class UISideMenuItemPermissionController : ApiControllerBase
    {
        [HttpPost("Create")]
        public async Task<ActionResult<long>> Create(CreateUISideMenuItemPermissionCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("Inactive/{id}")]
        public async Task<ActionResult> Inactive(long id)
        {
            await Mediator.Send(new InactiveUISideMenuItemPermissionCommand(id));

            return NoContent();
        }

        [HttpGet("Get-All")]
        public async Task<List<UISideMenuItemsPermissionsDto>> Get()
        {
            return await Mediator.Send(new GetUISideMenuItemsPermissionsQuery());
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(long id, UpdateUISideMenuItemPermissionCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }
    }
}
