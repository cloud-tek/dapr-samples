using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using Shared.Greeter;
using System.Threading.Tasks;

namespace Service2.Services
{
    public class GreeterService : IGreeterService
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public Task<HelloReply> SayHelloAsync(HelloRequest request, CallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
