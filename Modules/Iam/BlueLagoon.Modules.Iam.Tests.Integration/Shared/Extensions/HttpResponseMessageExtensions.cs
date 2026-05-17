using Newtonsoft.Json;

namespace BlueLagoon.Modules.Iam.Tests.Integration.Shared.Extensions;
public static class HttpResponseMessageExtensions
{
    public static TResult As<TResult>(this HttpResponseMessage message)
        where TResult : class
        => JsonConvert.DeserializeObject<TResult>(message.Content.ReadAsStringAsync().Result);

    public static async Task<Guid> ReadAsGuidAsync(this HttpContent content)
    {
        var json = await content.ReadAsStringAsync();
        var str = JsonConvert.DeserializeObject<string>(json);
        return Guid.Parse(str!);
    }
}
