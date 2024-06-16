using MyCoffeeShop.Application.Common.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace MyCoffeeShop.Application.Reports
{
    public class ReportController : ApiControllerBase
    {

        [HttpGet("TopSales")]
        [ProducesResponseType(typeof(List<TopSalesDto>), 200)]
        public async Task<ActionResult> ShopSales(
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(new TopSalesCommand(), cancellationToken));
        }


        [HttpPut("ShopSales")]
        [ProducesResponseType(typeof(List<ShopSalesDto>), 200)]
        public async Task<ActionResult> ShopSales(
            [FromBody] ShopSalesCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }

        [HttpPut("ShopOrders")]
        [ProducesResponseType(typeof(List<ShopOrdersDto>), 200)]
        public async Task<ActionResult> ShopOrders(
            [FromBody] ShopOrdersCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }

        [HttpPut("ProfitSixMonths")]
        [ProducesResponseType(typeof(ProfitSixMonthsDto), 200)]
        public async Task<ActionResult> ProfitSixMonths(
             [FromBody] ProfitSixMonthsCommand command, // Request body parameter
             CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }

        [HttpPut("DashboardInitial")]
        [ProducesResponseType(typeof(DashboardDto), 200)]
        public async Task<ActionResult> DashboardInitial(
            [FromBody] DashboardInitialCommand command, // Request body parameter
            CancellationToken cancellationToken // Cancellation token for async operation
        )
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }
    }
}
