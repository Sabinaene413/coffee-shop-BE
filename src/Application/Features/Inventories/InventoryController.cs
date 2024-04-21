using MyCoffeeShop.Application.Common.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace MyCoffeeShop.Application.Inventories
{
    public class InventoryController : ApiControllerBase
    {
        // API endpoint for creating a new Inventory
        [HttpPost("Create")]
        [ProducesResponseType(typeof(InventoryDto), 200)] // Specifies the response type for successful creation
        public async Task<ActionResult<InventoryDto>> Create(
            [FromBody] CreateInventoryCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return await Mediator.Send(command, cancellationToken); // Executes the command and returns the result
        }

        // API endpoint for deleting a Inventory by ID
        [HttpDelete("Delete")]
        [ProducesResponseType(204)] // Specifies the response type for successful deletion
        public async Task<ActionResult> Delete(
            int id, // Route parameter for Inventory ID
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            await Mediator.Send(new DeleteInventoryCommand { Id = id }, cancellationToken); // Executes the delete command
            return NoContent(); // Returns HTTP 204 No Content status
        }

        // API endpoint for getting a Inventory by ID
        [HttpPost("GetById")]
        [ProducesResponseType(typeof(InventoryDto), 200)] // Specifies the response type for successful retrieval
        public async Task<ActionResult<InventoryDto>> GetById(
            [FromBody] GetInventoryByIdCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the Inventory data
        }

        // API endpoint for filtering Inventories
        [HttpPost("Filter")]
        [ProducesResponseType(typeof(List<Inventory>), 200)] // Specifies the response type for successful filtering
        public async Task<ActionResult<List<Inventory>>> Filter(
            [FromBody] FilterInventorysCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the filtered list of Inventories
        }

        // API endpoint for updating a Inventory
        [HttpPut("Update")]
        [ProducesResponseType(typeof(InventoryDto), 200)] // Specifies the response type for successful update
        public async Task<ActionResult> Update(
            [FromBody] UpdateInventoryCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the updated Inventory data
        }
    }
}
