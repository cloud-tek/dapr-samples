using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using ProtoBuf.Grpc;
using Shared.Failing;
using Shared.Greeter;
using Xunit;

namespace Service2.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            GrpcClientFactory.AllowUnencryptedHttp2 = true;
            using (var channel = GrpcChannel.ForAddress("http://localhost:5000"))
            {
                var proxy = channel.CreateGrpcService<IGreeterService>();
                var result = await proxy.SayHelloAsync(new HelloRequest()
                {
                    Name = "Maciek"
                });
                result.GetType();
                //Console.WriteLine(result.Result);
            }
        }

        [Fact]
        public async Task Test2()
        {
            GrpcClientFactory.AllowUnencryptedHttp2 = true;
            using (var channel = GrpcChannel.ForAddress("http://localhost:5000"))
            {
                try
                {
                    var proxy = channel.CreateGrpcService<IFailingService>();
                    await proxy.Invoke();
                }
                catch (Exception ex)
                {
                    ex.GetType();
                }

                //Console.WriteLine(result.Result);
            }
        }
    }
}
