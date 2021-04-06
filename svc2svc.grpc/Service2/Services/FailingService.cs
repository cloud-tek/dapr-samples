using ProtoBuf.Grpc;
using Shared.Failing;
using System;
using System.Threading.Tasks;

namespace Service2.Services
{
    public class FailingService : IFailingService
    {
        public async Task Invoke(CallContext context = default)
        {
            await Task.Delay(1000);
            throw new Exception("Operation failed");
        }
    }
}
