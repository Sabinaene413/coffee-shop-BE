using MyCoffeeShop.Application.Common.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace MyCoffeeShop.Application.ShopOrders
{
    public class ShopOrderController : ApiControllerBase
    {
        // API endpoint for creating a new ShopOrder
        [HttpPost("Create")]
        [ProducesResponseType(typeof(ShopOrderDto), 200)] // Specifies the response type for successful creation
        public async Task<ActionResult<ShopOrderDto>> Create(
            [FromBody] CreateOrderCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return await Mediator.Send(command, cancellationToken); // Executes the command and returns the result
        }

        // API endpoint for deleting a ShopOrder by ID
        [HttpDelete("Delete")]
        [ProducesResponseType(204)] // Specifies the response type for successful deletion
        public async Task<ActionResult> Delete(
            int id, // Route parameter for ShopOrder ID
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            await Mediator.Send(new DeleteOrderCommand { Id = id }, cancellationToken); // Executes the delete command
            return NoContent(); // Returns HTTP 204 No Content status
        }

        // API endpoint for getting a ShopOrder by ID
        [HttpPost("GetById")]
        [ProducesResponseType(typeof(ShopOrderDto), 200)] // Specifies the response type for successful retrieval
        public async Task<ActionResult<ShopOrderDto>> GetById(
            [FromBody] GetOrderByIdCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the ShopOrder data
        }


        [HttpPost("MarkOrderReceived")]
        public async Task<ActionResult<ShopOrderDto>> MarkOrderReceived(
            [FromBody] MarkOrderReceivedCommand command,
            CancellationToken cancellationToken
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }

        // API endpoint for filtering ShopOrders
        [HttpPost("Filter")]
        [ProducesResponseType(typeof(List<ShopOrderFilterResponse>), 200)] // Specifies the response type for successful filtering
        public async Task<ActionResult<List<ShopOrderFilterResponse>>> Filter(
            [FromBody] FilterOrdersCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the filtered list of ShopOrders
        }

        // API endpoint for updating a ShopOrder
        [HttpPut("Update")]
        [ProducesResponseType(typeof(ShopOrderDto), 200)] // Specifies the response type for successful update
        public async Task<ActionResult> Update(
            [FromBody] UpdateOrderCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the updated ShopOrder data
        }
    }
}
