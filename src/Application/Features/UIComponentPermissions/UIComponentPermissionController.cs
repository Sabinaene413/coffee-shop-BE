using Microsoft.AspNetCore.Mvc;
using MyCoffeeShop.Application.Common.Abstracts;

namespace MyCoffeeShop.Application.UIComponentPermissions
{
    public class UIComponentPermissionController : ApiControllerBase
    {
        [HttpPost("Create")]
        public async Task<ActionResult<long>> Create(CreateUIComponentPermissionCommand command)
        {
            return await Mediator.Send(command);
        }


        [HttpPost("Inactive/{id}")]
        public async Task<ActionResult> Inactive(long id)
        {
            await Mediator.Send(new InactiveUIComponentPermissionCommand(id));

            return NoContent();
        }

        [HttpGet("Get-All")]
        public async Task<List<UIComponentPermissionDto>> Get()
        {
            return await Mediator.Send(new GetUIComponentPermissionsQuery());
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(long id, UpdateUIComponentPermissionCommand command)
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
