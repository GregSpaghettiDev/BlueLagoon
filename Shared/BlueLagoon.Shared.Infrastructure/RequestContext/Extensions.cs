using BlueLagoon.Shared.DevTools.Base;
using BlueLagoon.Shared.DevTools.Http;
using Microsoft.AspNetCore.Http;

namespace BlueLagoon.Shared.Infrastructure.RequestContext;

public static class Extensions
{
    public static BaseId GetUserId(this HttpContext httpContext)
        => httpContext.GetClaimValueAsBaseId("uid");
}
