namespace Shared
{
    public class Envelope<T>
        where T : class
    {
        public T Data { get; private set; }

        public uint Attempt { get; private set; } = 0;

        public Envelope(T data)
        {
            Data = data;
        }

        public Envelope<T> IncrementRetryCount()
        {
            Attempt++;
            return this;
        }
    }
}
