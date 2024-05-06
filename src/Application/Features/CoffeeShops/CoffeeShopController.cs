using MyCoffeeShop.Application.Common.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MyCoffeeShop.Application.CoffeeShops
{
    public class CoffeeShopController : ApiControllerBase
    {
        // API endpoint for creating a new CoffeeShop
        [HttpPost("Create")]
        [ProducesResponseType(typeof(CoffeeShopDto), 200)] // Specifies the response type for successful creation
        public async Task<ActionResult<CoffeeShopDto>> Create(
                [FromBody] CreateCoffeeShopCommand command, // Request body parameter
                CancellationToken cancellationToken // Cancellation token for async operation
            )
        {
            return await Mediator.Send(command, cancellationToken); // Executes the command and returns the result
        }

        // API endpoint for deleting a CoffeeShop by ID
        [HttpDelete("Delete")]
        [ProducesResponseType(204)] // Specifies the response type for successful deletion
        public async Task<ActionResult> Delete(
            int id, // Route parameter for CoffeeShop ID
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            await Mediator.Send(new DeleteCoffeeShopCommand { Id = id }, cancellationToken); // Executes the delete command
            return NoContent(); // Returns HTTP 204 No Content status
        }

        // API endpoint for getting a CoffeeShop by ID
        [HttpPost("GetById")]
        [ProducesResponseType(typeof(CoffeeShopDto), 200)] // Specifies the response type for successful retrieval
        public async Task<ActionResult<CoffeeShopDto>> GetById(
            [FromBody] GetCoffeeShopByIdCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the CoffeeShop data
        }

        // API endpoint for filtering CoffeeShops
        [HttpPost("Filter")]
        [ProducesResponseType(typeof(List<CoffeeShop>), 200)] // Specifies the response type for successful filtering
        public async Task<ActionResult<List<CoffeeShop>>> Filter(
            [FromBody] FilterCoffeeShopsCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the filtered list of CoffeeShops
        }

        // API endpoint for updating a CoffeeShop
        [HttpPut("Update")]
        [ProducesResponseType(typeof(CoffeeShopDto), 200)] // Specifies the response type for successful update
        public async Task<ActionResult> Update(
            [FromBody] UpdateCoffeeShopCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken)); // Returns the updated CoffeeShop data
        }
    }
}
