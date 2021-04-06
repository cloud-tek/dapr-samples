using System.ServiceModel;
using System.Threading.Tasks;
using ProtoBuf.Grpc;

namespace Shared.Greeter
{
    [ServiceContract(Name="Service2.GreeterService")]
    public interface IGreeterService
    {
        [OperationContract]
        Task<HelloReply> SayHelloAsync(HelloRequest request,
            CallContext context = default);
    }
}