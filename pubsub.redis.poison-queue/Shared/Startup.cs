using System;
using Microsoft.Extensions.DependencyInjection;
using Shared.Middleware;

namespace Shared
{
    public static class StartupExtensions
    {
        private const string PubSubKey = "pubsub";

        public static IServiceCollection AddCoreDaprServices(this IServiceCollection services)
        {
            services.AddSingleton<PubSubErrorHandlingStrategy>();

            services.AddSingleton<Func<string, IDaprErrorHandlingStrategy>>(provider =>
            {
                return (key) =>
                {
                    return key switch
                    {
                        PubSubKey => provider.GetRequiredService<PubSubErrorHandlingStrategy>(),
                        _ => throw new NotImplementedException()
                    };
                };
            });

            return services;
        }
    }
}
