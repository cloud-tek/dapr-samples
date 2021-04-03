using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared
{
    public static class StreamExtensions
    {
        private static JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreNullValues = true,
            WriteIndented = true,
        };

        static StreamExtensions()
        {
            options.Converters.Add(new JsonStringEnumConverter());
        }

        public static ValueTask<T> DeserializeAsync<T>(this Stream stream)
            where T : class
        {
            if(stream.CanSeek)
            {
                stream.Seek(0L, SeekOrigin.Begin);
            }

            return JsonSerializer.DeserializeAsync<T>(stream, options);
        }
    }
}
