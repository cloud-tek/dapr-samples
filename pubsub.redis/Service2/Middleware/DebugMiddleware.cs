
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Google.Rpc.Context;

namespace Service2.Middleware
{
    public class DebugMiddleware
    {
        private readonly RequestDelegate _next;

        public DebugMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var body = string.Empty;
            using (var reader = new StreamReader(context.Request.Body))
            {
                body = await reader.ReadToEndAsync();
                context.Request.Body.Seek(0L, SeekOrigin.Begin);
            }

            await _next(context);
        }
    }
}