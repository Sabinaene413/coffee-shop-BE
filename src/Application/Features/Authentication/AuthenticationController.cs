using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Features.Authentications.Business;

namespace MyCoffeeShop.Application.Features.Authentications
{
    public class AuthenticationController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginCommand command
           
        )
        {
            return Ok(await Mediator.Send(command));
        }

        [AllowAnonymous]
        [HttpPost("Refresh-Token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
