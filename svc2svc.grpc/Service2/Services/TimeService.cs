using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ProtoBuf.Grpc;

namespace Service2.Services
{
    public class TimeResult
    {
        public DateTime Time { get; set; }
    }

    public interface ITimeService
    {
        IAsyncEnumerable<TimeResult> SubscribeAsync(CallContext context);
    }

    public class TimeService : ITimeService
    {
        public IAsyncEnumerable<TimeResult> SubscribeAsync(CallContext context = default)
            => SubscribeAsync(context.CancellationToken);

        private async IAsyncEnumerable<TimeResult> SubscribeAsync([EnumeratorCancellation] CancellationToken cancel)
        {
            while (!cancel.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), cancel);
                yield return new TimeResult { Time = DateTime.UtcNow };
            }
        }
    }
}
