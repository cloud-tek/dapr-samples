using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Shared;

namespace Service1.Controllers
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
        [Route("api/order")]
        public async Task<IActionResult> ReceiveOrder([FromServices]DaprClient client, [FromBody] Order order)
        {
            if(order == null) {
                return BadRequest(order);
            }

            try {
                await client.PublishEventAsync("pubsub", "ordertopic", order);
                _logger.LogInformation($"Order with id {order.id} published");
            } catch(Exception ex) {
                _logger.LogInformation($"Failed to publish order id {order.id} : {ex.Message.ToString()} : {ex.InnerException?.Message.ToString()}", ex);
                return StatusCode(500);
            }
    
            return Ok();
        }
    }
}
