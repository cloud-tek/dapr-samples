using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Middleware
{
    public class DaprErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Func<string, IDaprErrorHandlingStrategy> provider;

        public DaprErrorHandlingMiddleware(RequestDelegate next, Func<string, IDaprErrorHandlingStrategy> provider)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.HasValue && context.Request.Path.Value.Contains(Constants.Dapr.DaprRequestPath))
            {
                await _next(context);
                return;
            }

            context.Request.EnableBuffering();

            try
            {
                await _next(context);
            }
            catch (Exception)
            {
                context.Request.Body.Seek(0L, SeekOrigin.Begin);

                var doc = await JsonDocument.ParseAsync(context.Request.Body);

                if (doc.RootElement.TryGetProperty(Constants.Dapr.PubSubNameProperty, out var val))
                {
                    await provider(Constants.Dapr.PubSub).HandleError(context, doc);
                    return;
                }
                
                throw new NotImplementedException();
            }
        }
    }
}