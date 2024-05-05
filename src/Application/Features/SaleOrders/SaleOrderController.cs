using MyCoffeeShop.Application.Common.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace MyCoffeeShop.Application.SaleOrders
{
    public class SaleOrderController : ApiControllerBase
    {
        // API endpoint for creating a new SaleOrder
        [HttpPost("Create")]
        [ProducesResponseType(typeof(SaleOrderDto), 200)] // Specifies the response type for successful creation
        public async Task<ActionResult<SaleOrderDto>> Create(
            [FromBody] CreateOrderCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return await Mediator.Send(command, cancellationToken); // Executes the command and returns the result
        }

        // API endpoint for deleting a SaleOrder by ID
        [HttpDelete("Delete")]
        [ProducesResponseType(204)] // Specifies the response type for successful deletion
        public async Task<ActionResult> Delete(
            int id, // Route parameter for SaleOrder ID
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            await Mediator.Send(new DeleteOrderCommand { Id = id }, cancellationToken); // Executes the delete command
            return NoContent(); // Returns HTTP 204 No Content status
        }

        // API endpoint for getting a SaleOrder by ID
        [HttpPost("GetById")]
        [ProducesResponseType(typeof(SaleOrderDto), 200)] // Specifies the response type for successful retrieval
        public async Task<ActionResult<SaleOrderDto>> GetById(
            [FromBody] GetOrderByIdCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the SaleOrder data
        }


        // API endpoint for filtering SaleOrders
        [HttpPost("Filter")]
        [ProducesResponseType(typeof(List<SaleOrder>), 200)] // Specifies the response type for successful filtering
        public async Task<ActionResult<List<SaleOrder>>> Filter(
            [FromBody] FilterOrdersCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the filtered list of SaleOrders
        }

        // API endpoint for updating a SaleOrder
        [HttpPut("Update")]
        [ProducesResponseType(typeof(SaleOrderDto), 200)] // Specifies the response type for successful update
        public async Task<ActionResult> Update(
            [FromBody] UpdateOrderCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the updated SaleOrder data
        }
    }
}
