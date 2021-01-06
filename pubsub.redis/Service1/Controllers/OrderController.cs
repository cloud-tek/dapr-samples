using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            await client.PublishEventAsync("pubsub", "ordertopic", order);
            _logger.LogInformation($"Order with id {order.id} published");
    
            return Ok();
        }
    }
}
