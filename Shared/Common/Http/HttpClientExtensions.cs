using Common.Jwt;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Common.Http;

public static class HttpClientExtensions
{
    private const string CookieHeaderName = "Cookie";

    public static Task<HttpResponseMessage> GetWithQueryStringAsync(this HttpClient client, string uri, Dictionary<string, string> queryStringParams, (string cookieName, string cookieValue)[] cookies = null)
    {
        if (cookies is not null && cookies.Length > 0)
            client.AddCookies(cookies);

        return client.GetAsync(QueryHelpers.AddQueryString(uri, queryStringParams));
    }

    public static Task<HttpResponseMessage> PostAsync(this HttpClient client, string uri, HttpContent content, (string cookieName, string cookieValue)[] cookies = null)
    {
        if (cookies is not null && cookies.Length > 0)
            client.AddCookies(cookies);

        return client.PostAsync(uri, content);
    }

    public static Task<HttpResponseMessage> PutAsync(this HttpClient client, string uri, HttpContent content, (string cookieName, string cookieValue)[] cookies = null)
    {
        if (cookies is not null && cookies.Length > 0)
            client.AddCookies(cookies);

        return client.PutAsync(uri, content);
    }

    public static Task<HttpResponseMessage> GetWithQueryStringAsync(this HttpClient client, string uri, Dictionary<string, string> queryStringParams)
        => client.GetAsync(QueryHelpers.AddQueryString(uri, queryStringParams));

    public static Task<HttpResponseMessage> PostWithMultipartFormDataAsync(this HttpClient client, string uri, MultipartFormDataContent formContent)
        => client
            .WithMultipartFormData()
            .PostAsync(uri, formContent);

    public static void ThrowExceptionIfRequestFailed(this HttpResponseMessage response, string message)
    {
        try
        {
            response.EnsureSuccessStatusCode();

        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException(message, ex?.InnerException ?? null, ex.StatusCode);
        }
    }

    public static HttpClient WithJwtBearerToken(this HttpClient client, string token)
    { 
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthenticationSchemes.Bearer, token); 

        return client;
    }

    public static HttpClient WithMultipartFormData(this HttpClient client)
    {
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data");

        return client;
    }

    private static HttpClient AddCookies(this HttpClient client, (string cookieName, string cookieValue)[] cookies)
    {
        if (client.DefaultRequestHeaders.Any(x => x.Key == CookieHeaderName))
            client.DefaultRequestHeaders.Remove(CookieHeaderName);

        StringBuilder cookieHeaderBuilder = new();
        foreach (var (cookieName, cookieValue) in cookies)
            cookieHeaderBuilder.Append($"{cookieName}={cookieValue};");

        client.DefaultRequestHeaders.Add(CookieHeaderName, cookieHeaderBuilder.ToString());

        return client;
    }
}