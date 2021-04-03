using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Shared.Middleware
{
    public sealed class RequestLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestLoggingMiddleware> logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var start = Stopwatch.GetTimestamp();
            try
            {
                await next(context);
                var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());

                var statusCode = context.Response?.StatusCode;
                if (statusCode < 500)
                {
                    logger.LogInformation("HTTP {0} {1} responded with {1} in {3:0.0000} [ms]",
                        context.Request.Method, context.Request.Path, context.Response.StatusCode, elapsedMs);
                }
                else
                {
                    logger.LogError("HTTP {0} {1} responded with {2} in {3:0.0000} [ms]",
                        context.Request.Method, context.Request.Path, context.Response.StatusCode, elapsedMs);
                }
            }
            catch (Exception ex) when (LogException(context, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()), ex)) { }
        }

        private bool LogException(HttpContext context, double elapsedMs, Exception ex)
        {
            logger.LogError("HTTP {0} {1} failed with {2} in {3:0.0000} [ms] : {4}",
                context.Request.Method, context.Request.Path, context.Response.StatusCode, elapsedMs, ex.Message);

            return false;
        }

        private static double GetElapsedMilliseconds(long start, long stop)
        {
            return ((stop - start) * 1000 / (double)Stopwatch.Frequency);
        }
    }
}
