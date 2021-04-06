using System.Runtime.Serialization;

namespace Shared.Greeter
{
    [DataContract]
    public class HelloReply
    {
        [DataMember(Order = 1)]
        public string Message { get; set; }
    }
}