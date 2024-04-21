using MyCoffeeShop.Application.Common.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace MyCoffeeShop.Application.Products
{
    public class ProductController : ApiControllerBase
    {
        // API endpoint for creating a new Product
        [HttpPost("Create")]
        [ProducesResponseType(typeof(ProductDto), 200)] // Specifies the response type for successful creation
        public async Task<ActionResult<ProductDto>> Create(
            [FromBody] CreateProductCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return await Mediator.Send(command, cancellationToken); // Executes the command and returns the result
        }

        // API endpoint for deleting a Product by ID
        [HttpDelete("Delete")]
        [ProducesResponseType(204)] // Specifies the response type for successful deletion
        public async Task<ActionResult> Delete(
            int id, // Route parameter for Product ID
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            await Mediator.Send(new DeleteProductCommand { Id = id }, cancellationToken); // Executes the delete command
            return NoContent(); // Returns HTTP 204 No Content status
        }

        // API endpoint for getting a Product by ID
        [HttpPost("GetById")]
        [ProducesResponseType(typeof(ProductDto), 200)] // Specifies the response type for successful retrieval
        public async Task<ActionResult<ProductDto>> GetById(
            [FromBody] GetProductByIdCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the Product data
        }

        // API endpoint for filtering Products
        [HttpPost("Filter")]
        [ProducesResponseType(typeof(List<Product>), 200)] // Specifies the response type for successful filtering
        public async Task<ActionResult<List<Product>>> Filter(
            [FromBody] FilterProductsCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the filtered list of Products
        }

        // API endpoint for updating a Product
        [HttpPut("Update")]
        [ProducesResponseType(typeof(ProductDto), 200)] // Specifies the response type for successful update
        public async Task<ActionResult> Update(
            [FromBody] UpdateProductCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the updated Product data
        }
    }
}
