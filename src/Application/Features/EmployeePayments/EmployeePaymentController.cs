using MyCoffeeShop.Application.Common.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MyCoffeeShop.Application.EmployeePayments
{
    public class EmployeePaymentController : ApiControllerBase
    {
        // API endpoint for creating a new EmployeePayment
        [HttpPost("Create")]
        [ProducesResponseType(typeof(EmployeePaymentDto), 200)] // Specifies the response type for successful creation
        public async Task<ActionResult<EmployeePaymentDto>> Create(
                [FromBody] CreateEmployeePaymentCommand command, // Request body parameter
                CancellationToken cancellationToken // Cancellation token for async operation
            )
        {
            return await Mediator.Send(command, cancellationToken); // Executes the command and returns the result
        }

        // API endpoint for deleting a EmployeePayment by ID
        [HttpDelete("Delete")]
        [ProducesResponseType(204)] // Specifies the response type for successful deletion
        public async Task<ActionResult> Delete(
            int id, // Route parameter for EmployeePayment ID
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            await Mediator.Send(new DeleteEmployeePaymentCommand { Id = id }, cancellationToken); // Executes the delete command
            return NoContent(); // Returns HTTP 204 No Content status
        }

        // API endpoint for getting a EmployeePayment by ID
        [HttpPost("GetById")]
        [ProducesResponseType(typeof(EmployeePaymentDto), 200)] // Specifies the response type for successful retrieval
        public async Task<ActionResult<EmployeePaymentDto>> GetById(
            [FromBody] GetEmployeePaymentByIdCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the EmployeePayment data
        }

        // API endpoint for filtering EmployeePayments
        [AllowAnonymous]
        [HttpPost("Filter")]
        [ProducesResponseType(typeof(List<EmployeePayment>), 200)] // Specifies the response type for successful filtering
        public async Task<ActionResult<List<EmployeePayment>>> Filter(
            [FromBody] FilterEmployeePaymentsCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the filtered list of EmployeePayments
        }

        // API endpoint for updating a EmployeePayment
        [HttpPut("Update")]
        [ProducesResponseType(typeof(EmployeePaymentDto), 200)] // Specifies the response type for successful update
        public async Task<ActionResult> Update(
            [FromBody] UpdateEmployeePaymentCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the updated EmployeePayment data
        }
    }
}
