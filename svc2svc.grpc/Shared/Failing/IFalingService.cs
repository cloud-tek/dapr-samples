using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Shared.Failing
{
    [ServiceContract(Name = nameof(IFailingService))]
    public interface IFailingService
    {
        [OperationContract]
        Task Invoke(CallContext context = default);
    }
}
