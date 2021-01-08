using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dapr;
using Microsoft.Extensions.Logging;
using Shared;

namespace Service3
{
    public class Startup
    {
        private ILogger<Startup> logger;

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            logger = app.ApplicationServices.GetService<ILoggerFactory>().CreateLogger<Startup>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints
                    .MapSubscribeHandler();

                endpoints.MapGet("/", async context =>
                {
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync("Service3");
                });
                
                endpoints
                    .MapPost("/ordertopic", async context =>
                {
                    var order = await JsonSerializer.DeserializeAsync<Order>(context.Request.Body);
                    logger.LogInformation($"Order with id {order.id} received");
                    
                    context.Response.StatusCode = 200;
                    
                }).WithMetadata(new TopicAttribute("pubsub", "ordertopic"));
            });
        }
    }
}
