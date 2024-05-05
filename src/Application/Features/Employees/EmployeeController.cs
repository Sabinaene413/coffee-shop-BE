using MyCoffeeShop.Application.Common.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MyCoffeeShop.Application.Employees
{
    public class EmployeeController : ApiControllerBase
    {
        // API endpoint for creating a new Employee
        [HttpPost("Create")]
        [ProducesResponseType(typeof(EmployeeDto), 200)] // Specifies the response type for successful creation
        public async Task<ActionResult<EmployeeDto>> Create(
                [FromForm] CreateEmployeeCommand command, // Request body parameter
                CancellationToken cancellationToken // Cancellation token for async operation
            )
        {
            return await Mediator.Send(command, cancellationToken); // Executes the command and returns the result
        }

        // API endpoint for deleting a Employee by ID
        [HttpDelete("Delete")]
        [ProducesResponseType(204)] // Specifies the response type for successful deletion
        public async Task<ActionResult> Delete(
            int id, // Route parameter for Employee ID
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            await Mediator.Send(new DeleteEmployeeCommand { Id = id }, cancellationToken); // Executes the delete command
            return NoContent(); // Returns HTTP 204 No Content status
        }

        // API endpoint for getting a Employee by ID
        [HttpPost("GetById")]
        [ProducesResponseType(typeof((EmployeeDto, IFormFile)), 200)] // Specifies the response type for successful retrieval
        public async Task<ActionResult<(EmployeeDto, IFormFile)>> GetById(
            [FromBody] GetEmployeeByIdCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the Employee data
        }

        // API endpoint for filtering Employees
        [HttpPost("Filter")]
        [ProducesResponseType(typeof(List<Employee>), 200)] // Specifies the response type for successful filtering
        public async Task<ActionResult<List<Employee>>> Filter(
            [FromBody] FilterEmployeesCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the filtered list of Employees
        }

        // API endpoint for updating a Employee
        [HttpPut("Update")]
        [ProducesResponseType(typeof(EmployeeDto), 200)] // Specifies the response type for successful update
        public async Task<ActionResult> Update(
            [FromBody] UpdateEmployeeCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the updated Employee data
        }
    }
}
