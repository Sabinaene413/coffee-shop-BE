using MyCoffeeShop.Application.Common.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace MyCoffeeShop.Application.Invoices
{
    public class InvoiceController : ApiControllerBase
    {
        // API endpoint for creating a new Invoice
        [HttpPost("Create")]
        [ProducesResponseType(typeof(InvoiceDto), 200)] // Specifies the response type for successful creation
        public async Task<ActionResult<InvoiceDto>> Create(
            [FromBody] CreateInvoiceCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return await Mediator.Send(command, cancellationToken); // Executes the command and returns the result
        }

        // API endpoint for deleting a Invoice by ID
        [HttpDelete("Delete")]
        [ProducesResponseType(204)] // Specifies the response type for successful deletion
        public async Task<ActionResult> Delete(
            int id, // Route parameter for Invoice ID
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            await Mediator.Send(new DeleteInvoiceCommand { Id = id }, cancellationToken); // Executes the delete command
            return NoContent(); // Returns HTTP 204 No Content status
        }

        // API endpoint for getting a Invoice by ID
        [HttpPost("GetById")]
        [ProducesResponseType(typeof(InvoiceDto), 200)] // Specifies the response type for successful retrieval
        public async Task<ActionResult<InvoiceDto>> GetById(
            [FromBody] GetInvoiceByIdCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the Invoice data
        }

        // API endpoint for filtering Invoices
        [HttpPost("Filter")]
        [ProducesResponseType(typeof(List<Invoice>), 200)] // Specifies the response type for successful filtering
        public async Task<ActionResult<List<Invoice>>> Filter(
            [FromBody] FilterInvoicesCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the filtered list of Invoices
        }

        // API endpoint for updating a Invoice
        [HttpPut("Update")]
        [ProducesResponseType(typeof(InvoiceDto), 200)] // Specifies the response type for successful update
        public async Task<ActionResult> Update(
            [FromBody] UpdateInvoiceCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the updated Invoice data
        }
    }
}
