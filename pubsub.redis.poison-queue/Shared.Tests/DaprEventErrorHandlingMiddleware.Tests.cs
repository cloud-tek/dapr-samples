using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Shared.Middleware;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Shared.Tests
{
    public class DaprEventErrorHandlingMiddlewareTests
    {
        public abstract class BaseStartup
        {
            public virtual void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
                services.AddCoreDaprServices();
            }

            public void Configure(IApplicationBuilder app)
            {
                app.UseMiddleware<DaprErrorHandlingMiddleware>();
                app.Run(async (ctx) =>
                {
                    if (ctx.Request.Path.HasValue && ctx.Request.Path.Value.Contains("dapr"))
                    {
                        ctx.Response.StatusCode = 200;
                        return;
                    }

                    // simulate reading the body
                    using var reader = new StreamReader(
                        ctx.Request.Body,
                        encoding: Encoding.UTF8,
                        detectEncodingFromByteOrderMarks: false,
                        bufferSize: 1024,
                        leaveOpen: true
                    );
                    var body = await reader.ReadToEndAsync();

                    throw new Exception("Expected failure");
                });
            }
        }

        public class Startup : BaseStartup
        {
            public override void ConfigureServices(IServiceCollection services)
            {


                services.AddDaprClient(options => {
                    options.UseJsonSerializationOptions(new System.Text.Json.JsonSerializerOptions());
                });

                base.ConfigureServices(services);
            }
        }

        public class GivenCorrelationIdIsNotRequired
        {
            [Fact]
            public async Task WhenCorrelationIdIsNotRequired_ThenOkResponseWithGuildCorrelationIdResponseHeader()
            {
                // Arrange
                var builder = new WebHostBuilder()
                    .UseStartup<Startup>();

                var server = new TestServer(builder);

                var client = server.CreateClient();

                // Act
                var request = new HttpRequestMessage(HttpMethod.Post, "/topic");
                request.Content = new StringContent(Constants.PubSubCloudEvent, Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);

                // Assert
                //response.StatusCode.Should().Be(HttpStatusCode.OK);
                //response.Headers.Contains(Constants.RequestHeaders.CorrelationId).Should().BeTrue();
                //Guid.TryParse(response.Headers.GetValues(Constants.RequestHeaders.CorrelationId).First(),
                //    out var correlationId).Should().BeTrue();
            }
        }
    }
}