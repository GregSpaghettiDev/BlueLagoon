using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace BlueLagoon.Shared.DevTools.Http;

public static class HttpClientExtensions
{
    private const string JwtSchemeName = "Bearer";

    public static Task<HttpResponseMessage> DeleteAsync(this HttpClient client, string uri, params (string Key, string Value)[] queryStringParams)
    {
        var dictionary = queryStringParams.ToDictionary(p => p.Key, p => p.Value);
        var request = new HttpRequestMessage(HttpMethod.Delete, QueryHelpers.AddQueryString(uri, dictionary));

        return client.SendAsync(request);
    }

    public static Task<HttpResponseMessage> GetWithQueryStringAsync(this HttpClient client, string uri, params (string Key, string Value)[] queryStringParams)
    {
        var dictionary = queryStringParams.ToDictionary(p => p.Key, p => p.Value);
        var request = new HttpRequestMessage(HttpMethod.Get, QueryHelpers.AddQueryString(uri, dictionary));

        return client.SendAsync(request);
    }

    public static Task<HttpResponseMessage> GetAsync(this HttpClient client, string uri)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        return client.SendAsync(request);
    }

    public static Task<HttpResponseMessage> PostAsync(this HttpClient client, string uri, HttpContent content)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = content
        };

        return client.SendAsync(request);
    }

    public static Task<HttpResponseMessage> PutAsync(this HttpClient client, string uri, HttpContent content)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, uri)
        {
            Content = content
        };

        return client.SendAsync(request);
    }

    public static Task<HttpResponseMessage> PostWithMultipartFormDataAsync(this HttpClient client, string uri, MultipartFormDataContent formContent)
        => client
            .WithMultipartFormData()
            .PostAsync(uri, formContent);

    public static HttpClient WithJwtBearerToken(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtSchemeName, token);

        return client;
    }

    public static HttpClient WithMultipartFormData(this HttpClient client)
    {
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

        return client;
    }

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