using MyCoffeeShop.Application.Common.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace MyCoffeeShop.Application.ShopProducts
{
    public class ShopProductController : ApiControllerBase
    {
        // API endpoint for creating a new ShopProduct
        [HttpPost("Create")]
        [ProducesResponseType(typeof(ShopProductDto), 200)] // Specifies the response type for successful creation
        public async Task<ActionResult<ShopProductDto>> Create(
            [FromBody] CreateShopProductCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return await Mediator.Send(command, cancellationToken); // Executes the command and returns the result
        }

        // API endpoint for deleting a ShopProduct by ID
        [HttpDelete("Delete")]
        [ProducesResponseType(204)] // Specifies the response type for successful deletion
        public async Task<ActionResult> Delete(
            int id, // Route parameter for ShopProduct ID
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            await Mediator.Send(new DeleteShopProductCommand { Id = id }, cancellationToken); // Executes the delete command
            return NoContent(); // Returns HTTP 204 No Content status
        }

        // API endpoint for getting a ShopProduct by ID
        [HttpPost("GetById")]
        [ProducesResponseType(typeof(ShopProductDto), 200)] // Specifies the response type for successful retrieval
        public async Task<ActionResult<ShopProductDto>> GetById(
            [FromBody] GetShopProductByIdCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the ShopProduct data
        }

        // API endpoint for filtering ShopOrderProducts
        [HttpPost("Filter")]
        [ProducesResponseType(typeof(List<ShopProduct>), 200)] // Specifies the response type for successful filtering
        public async Task<ActionResult<List<ShopProduct>>> Filter(
            [FromBody] FilterShopProductsCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the filtered list of ShopOrderProducts
        }

        // API endpoint for updating a ShopProduct
        [HttpPut("Update")]
        [ProducesResponseType(typeof(ShopProductDto), 200)] // Specifies the response type for successful update
        public async Task<ActionResult> Update(
            [FromBody] UpdateShopProductCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the updated ShopProduct data
        }
    }
}
