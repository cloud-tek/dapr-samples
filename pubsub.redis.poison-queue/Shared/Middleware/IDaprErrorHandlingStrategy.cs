using System.Text.Json;
using Dapr.Client;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Shared.Middleware
{
    public interface IDaprErrorHandlingStrategy
    {
        Task HandleError(HttpContext context, JsonDocument cloudEvent);
    }
}
