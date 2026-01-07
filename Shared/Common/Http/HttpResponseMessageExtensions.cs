using Newtonsoft.Json;
using System.Net.Http;

namespace Common.Http;

public static class HttpResponseMessageExtensions
{
    public static TResult As<TResult>(this HttpResponseMessage message)
        where TResult : class
        => JsonConvert.DeserializeObject<TResult>(message.Content.ReadAsStringAsync().Result);
}