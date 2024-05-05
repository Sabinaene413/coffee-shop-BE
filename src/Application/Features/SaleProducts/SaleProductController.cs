using MyCoffeeShop.Application.Common.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace MyCoffeeShop.Application.SaleProducts
{
    public class SaleProductController : ApiControllerBase
    {
        // API endpoint for creating a new SaleProduct
        [HttpPost("Create")]
        [ProducesResponseType(typeof(SaleProductDto), 200)] // Specifies the response type for successful creation
        public async Task<ActionResult<SaleProductDto>> Create(
            [FromBody] CreateSaleProductCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return await Mediator.Send(command, cancellationToken); // Executes the command and returns the result
        }

        // API endpoint for deleting a SaleProduct by ID
        [HttpDelete("Delete")]
        [ProducesResponseType(204)] // Specifies the response type for successful deletion
        public async Task<ActionResult> Delete(
            int id, // Route parameter for SaleProduct ID
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            await Mediator.Send(new DeleteSaleProductCommand { Id = id }, cancellationToken); // Executes the delete command
            return NoContent(); // Returns HTTP 204 No Content status
        }

        // API endpoint for getting a SaleProduct by ID
        [HttpPost("GetById")]
        [ProducesResponseType(typeof(SaleProductDto), 200)] // Specifies the response type for successful retrieval
        public async Task<ActionResult<SaleProductDto>> GetById(
            [FromBody] GetSaleProductByIdCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the SaleProduct data
        }

        // API endpoint for filtering ShopOrderProducts
        [HttpPost("Filter")]
        [ProducesResponseType(typeof(List<SaleProduct>), 200)] // Specifies the response type for successful filtering
        public async Task<ActionResult<List<SaleProduct>>> Filter(
            [FromBody] FilterSaleProductsCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the filtered list of ShopOrderProducts
        }

        // API endpoint for updating a SaleProduct
        [HttpPut("Update")]
        [ProducesResponseType(typeof(SaleProductDto), 200)] // Specifies the response type for successful update
        public async Task<ActionResult> Update(
            [FromBody] UpdateSaleProductCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the updated SaleProduct data
        }
    }
}
