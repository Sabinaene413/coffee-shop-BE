using MyCoffeeShop.Application.Common.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace MyCoffeeShop.Application.Orders
{
    public class OrderController : ApiControllerBase
    {
        // API endpoint for creating a new Order
        [HttpPost("Create")]
        [ProducesResponseType(typeof(OrderDto), 200)] // Specifies the response type for successful creation
        public async Task<ActionResult<OrderDto>> Create(
            [FromBody] CreateOrderCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return await Mediator.Send(command, cancellationToken); // Executes the command and returns the result
        }

        // API endpoint for deleting a Order by ID
        [HttpDelete("Delete")]
        [ProducesResponseType(204)] // Specifies the response type for successful deletion
        public async Task<ActionResult> Delete(
            int id, // Route parameter for Order ID
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            await Mediator.Send(new DeleteOrderCommand { Id = id }, cancellationToken); // Executes the delete command
            return NoContent(); // Returns HTTP 204 No Content status
        }

        // API endpoint for getting a Order by ID
        [HttpPost("GetById")]
        [ProducesResponseType(typeof(OrderDto), 200)] // Specifies the response type for successful retrieval
        public async Task<ActionResult<OrderDto>> GetById(
            [FromBody] GetOrderByIdCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the Order data
        }

        // API endpoint for filtering Orders
        [HttpPost("Filter")]
        [ProducesResponseType(typeof(List<Order>), 200)] // Specifies the response type for successful filtering
        public async Task<ActionResult<List<Order>>> Filter(
            [FromBody] FilterOrdersCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the filtered list of Orders
        }

        // API endpoint for updating a Order
        [HttpPut("Update")]
        [ProducesResponseType(typeof(OrderDto), 200)] // Specifies the response type for successful update
        public async Task<ActionResult> Update(
            [FromBody] UpdateOrderCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the updated Order data
        }
    }
}
