using Dapr.Client;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Middleware
{
    public class PubSubErrorHandlingStrategy : IDaprErrorHandlingStrategy
    {
        private readonly DaprClient client;
        public PubSubErrorHandlingStrategy(DaprClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }
        public Task HandleError(HttpContext context, JsonDocument cloudElement)
        {
            

            throw new NotImplementedException();
        }
    }
}
