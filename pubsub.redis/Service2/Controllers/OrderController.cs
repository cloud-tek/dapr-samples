using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Shared;

using System.Threading.Tasks;

namespace Service2.Controllers
{
    //[Route("api/[controller]")]
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
        public async Task<IActionResult> ReceiveOrder([FromServices]DaprClient client, [FromBody] Order order)
        {
            _logger.LogInformation($"Order with id {order.id} received");

            //if(System.Int32.Parse(order.id) % 2 == 0) {
            //    throw new Exception("An error has occurred");
            //}

            return Ok();
        }
    }
}
