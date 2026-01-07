using Common.BaseValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Common.Http
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            return httpContext?.User?.Claims?.SingleOrDefault(x => x.Type == "id")?.Value ?? string.Empty;
        }

        public static Guid GetUserIdAsGuid(this HttpContext httpContext)
        {
            var userId = httpContext?.User?.Claims?.SingleOrDefault(x => x.Type == "id")?.Value ?? string.Empty;
            if (Guid.TryParse(userId, out var parsedUserId))
            {
                return parsedUserId;
            }

            return Guid.Empty;
        }

        public static BaseId GetClaimValueAsBaseId(this HttpContext httpContext, string claimName)
        {
            var value = httpContext?.User?.Claims?.SingleOrDefault(x => x.Type == claimName)?.Value ?? string.Empty;
            if (Guid.TryParse(value, out Guid parsedValue))
            {
                return new BaseId(parsedValue);
            }

            return null;
        }

        public static string GetClaimValueByName(this HttpContext httpContext, string claimName)
        {
            var result = httpContext.User.Claims.SingleOrDefault(x => x.Type == claimName)?.Value ?? string.Empty;

            return result;
        }

        public static List<string> GetClaimValuesByName(this HttpContext httpContext, string claimName)
        {
            return httpContext.User.Claims.Where(x => x.Type == claimName).Select(x => x.Value).ToList();
        }

        public static (string, string) GetAuthorizationHeader(this HttpContext httpContext)
        {
            var authorizationHeader = httpContext.Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorizationHeader, out var headerValue))
            {
                var scheme = headerValue.Scheme;
                var param = headerValue.Parameter;

                return (scheme, param);
            }

            return (string.Empty, string.Empty);
        }

        public static void AddCreatedResourceId(this HttpContext httpContext, Guid resourceId)
        {
            httpContext?.Items.Add(nameof(CreatedResource.CreatedResourceId), resourceId.ToString());
        }

        public static void AddOkStatusCode(this HttpContext httpContext)
        {
            httpContext?.Items.Add(nameof(StatusCodeResponse.StatusCode), "200");
        }

        public static void AddBadRequestStatusCode(this HttpContext httpContext)
        {
            httpContext?.Items.Add(nameof(StatusCodeResponse.StatusCode), "400");
        }
    }

    public class CreatedResource
    {
        public Guid CreatedResourceId { get; set; }
    }

    public class StatusCodeResponse
    {
        public int StatusCode { get; set; }
    }
}