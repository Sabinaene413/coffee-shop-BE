using MyCoffeeShop.Application.Common.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MyCoffeeShop.Application.Transactions
{
    public class TransactionController : ApiControllerBase
    {
        // API endpoint for creating a new Transaction
        [HttpPost("Create")]
        [ProducesResponseType(typeof(TransactionDto), 200)] // Specifies the response type for successful creation
        public async Task<ActionResult<TransactionDto>> Create(
                [FromBody] CreateTransactionCommand command, // Request body parameter
                CancellationToken cancellationToken // Cancellation token for async operation
            )
        {
            return await Mediator.Send(command, cancellationToken); // Executes the command and returns the result
        }

        // API endpoint for deleting a Transaction by ID
        [HttpDelete("Delete")]
        [ProducesResponseType(204)] // Specifies the response type for successful deletion
        public async Task<ActionResult> Delete(
            int id, // Route parameter for Transaction ID
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            await Mediator.Send(new DeleteTransactionCommand { Id = id }, cancellationToken); // Executes the delete command
            return NoContent(); // Returns HTTP 204 No Content status
        }

        // API endpoint for getting a Transaction by ID
        [HttpPost("GetById")]
        [ProducesResponseType(typeof(TransactionDto), 200)] // Specifies the response type for successful retrieval
        public async Task<ActionResult<TransactionDto>> GetById(
            [FromBody] GetTransactionByIdCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the Transaction data
        }

        // API endpoint for filtering Transactions
        [AllowAnonymous]
        [HttpPost("Filter")]
        [ProducesResponseType(typeof(List<Transaction>), 200)] // Specifies the response type for successful filtering
        public async Task<ActionResult<List<Transaction>>> Filter(
            [FromBody] FilterTransactionsCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the filtered list of Transactions
        }

        // API endpoint for updating a Transaction
        [HttpPut("Update")]
        [ProducesResponseType(typeof(TransactionDto), 200)] // Specifies the response type for successful update
        public async Task<ActionResult> Update(
            [FromBody] UpdateTransactionCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the updated Transaction data
        }
    }
}
