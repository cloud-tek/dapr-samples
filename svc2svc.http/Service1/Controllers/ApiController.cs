using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dapr.Client;
using Dapr.Client.Autogen.Grpc.v1;

namespace Service1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
      private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> InvokeService2()
        {
            var client = DaprClient.CreateInvokeHttpClient("service-2");

            return (await client.GetAsync("/weatherforecast"))
                .AsHttpResponseMessageResult();
        }
    }
}
