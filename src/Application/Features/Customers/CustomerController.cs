using MyCoffeeShop.Application.Common.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace MyCoffeeShop.Application.Customers
{
    public class CustomerController : ApiControllerBase
    {
        // API endpoint for creating a new Customer
        [HttpPost("Create")]
        [ProducesResponseType(typeof(CustomerDto), 200)] // Specifies the response type for successful creation
        public async Task<ActionResult<CustomerDto>> Create(
            [FromBody] CreateCustomerCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return await Mediator.Send(command, cancellationToken); // Executes the command and returns the result
        }

        // API endpoint for deleting a Customer by ID
        [HttpDelete("Delete")]
        [ProducesResponseType(204)] // Specifies the response type for successful deletion
        public async Task<ActionResult> Delete(
            int id, // Route parameter for Customer ID
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            await Mediator.Send(new DeleteCustomerCommand { Id = id }, cancellationToken); // Executes the delete command
            return NoContent(); // Returns HTTP 204 No Content status
        }

        // API endpoint for getting a Customer by ID
        [HttpPost("GetById")]
        [ProducesResponseType(typeof(CustomerDto), 200)] // Specifies the response type for successful retrieval
        public async Task<ActionResult<CustomerDto>> GetById(
            [FromBody] GetCustomerByIdCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the Customer data
        }

        // API endpoint for filtering Customers
        [HttpPost("Filter")]
        [ProducesResponseType(typeof(List<Customer>), 200)] // Specifies the response type for successful filtering
        public async Task<ActionResult<List<Customer>>> Filter(
            [FromBody] FilterCustomersCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the filtered list of Customers
        }

        // API endpoint for updating a Customer
        [HttpPut("Update")]
        [ProducesResponseType(typeof(CustomerDto), 200)] // Specifies the response type for successful update
        public async Task<ActionResult> Update(
            [FromBody] UpdateCustomerCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the updated Customer data
        }
    }
}
