using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Features.Users.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MyCoffeeShop.Application.Users
{
    public class UserController : ApiControllerBase
    {
        [AllowAnonymous]
        [HttpPost("Register")]
        [ProducesResponseType(typeof(UserDto), 200)] // Specifies the response type for successful creation
        public async Task<ActionResult<UserDto>> Register(
            [FromBody] RegisterUserCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return await Mediator.Send(command, cancellationToken); // Executes the command and returns the result
        }

        [HttpPost("EmployeeRegister")]
        [ProducesResponseType(typeof(UserDto), 200)] // Specifies the response type for successful creation
        public async Task<ActionResult<UserDto>> EmployeeRegister(
            [FromBody] EmployeeRegisterUserCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return await Mediator.Send(command, cancellationToken); // Executes the command and returns the result
        }


        // API endpoint for creating a new user
        [HttpPost("Create")]
        [ProducesResponseType(typeof(UserDto), 200)] // Specifies the response type for successful creation
        public async Task<ActionResult<UserDto>> Create(
            [FromBody] CreateUserCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return await Mediator.Send(command, cancellationToken); // Executes the command and returns the result
        }

        // API endpoint for deleting a user by ID
        [HttpDelete("Delete")]
        [ProducesResponseType(204)] // Specifies the response type for successful deletion
        public async Task<ActionResult> Delete(
            int id, // Route parameter for user ID
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            await Mediator.Send(new DeleteUserCommand { Id = id }, cancellationToken); // Executes the delete command
            return NoContent(); // Returns HTTP 204 No Content status
        }

        // API endpoint for getting a user by ID
        [HttpPost("GetById")]
        [ProducesResponseType(typeof(UserDto), 200)] // Specifies the response type for successful retrieval
        public async Task<ActionResult<UserDto>> GetById(
            [FromBody] GetUserByIdCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the user data
        }

        // API endpoint for filtering users
        [HttpPost("Filter")]
        [ProducesResponseType(typeof(List<User>), 200)] // Specifies the response type for successful filtering
        public async Task<ActionResult<List<User>>> Filter(
            [FromBody] FilterUsersCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the filtered list of users
        }

        [AllowAnonymous]
        // API endpoint for updating a user
        [HttpPut("Update")]
        [ProducesResponseType(typeof(UserDto), 200)] // Specifies the response type for successful update
        public async Task<ActionResult> Update(
            [FromBody] UpdateUserCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the updated user data
        }
    }
}
