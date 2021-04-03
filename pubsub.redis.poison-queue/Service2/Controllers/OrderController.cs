using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;
using System;
using System.Threading.Tasks;

namespace Service2.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("ordertopic")]
        [Topic("pubsub", "ordertopic")]
        public async Task<IActionResult> ReceiveOrder([FromServices]DaprClient client, [FromBody] Envelope<Order> order)
        {
            _logger.LogInformation($"Order with id {order.Data.id} received");

            //await Task.FromResult(0);

            //return Ok();

            throw new InvalidOperationException("Operation failed");
        }
    }
}
