using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using System.Security.Claims;

namespace BlueLagoon.Modules.Iam.Core.Services;

internal sealed class AuthorizationService(IHttpContextAccessor contextAccessor,
                                           SignInManager<User> signInManager,
                                           UserManager<User> userManager) 
    : IAuthorizationService
{
    private readonly HttpContext httpContext = contextAccessor.HttpContext;

    public async Task<AuthorizationResultDto> AuthorizeAsync()
    {
        var request = httpContext.GetOpenIddictServerRequest();
        if (request is null)
            throw new InvalidOperationException("Nie można pobrać żądania OpenId Connect.");

        var returnUrl = httpContext.Request.PathBase + httpContext.Request.Path + httpContext.Request.QueryString;

        if (!httpContext.User.Identity?.IsAuthenticated ?? true)
            return new AuthorizationResultDto
            {
                IsChallenge = true,
                RedirectUri = returnUrl
            };

        var userId = httpContext.User; 
        var user = await userManager.FindByIdAsync("");
        var principal = await signInManager.CreateUserPrincipalAsync(user);
        
        var userAllowedScopes = await GetAuthorizedModuleScopesAsync(principal);
        principal.SetScopes(userAllowedScopes);

        foreach (var claim in principal.Claims)
            claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken);

        return new AuthorizationResultDto
        {
            IsChallenge = false,
            Principal = principal
        };
    }

    public async Task<IEnumerable<string>> GetAuthorizedModuleScopesAsync(ClaimsPrincipal principal)
    {
        var permissions = principal.Claims
                                    .Where(c => c.Type == "permission")
                                    .Select(c => c.Value)
                                    .ToList();

        var authorizedModules = permissions
                                    .Select(p => p.Split('.')[0])
                                    .Distinct()
                                    .ToList();

        if (principal.IsInRole(ValueObjects.Role.IamAdmin))
            authorizedModules.Add(ValueObjects.Role.IamAdmin);


        return authorizedModules;
    }

    public async Task<string> LogoutAsync()
    {
        var request = httpContext.GetOpenIddictServerRequest();
        if (request is null)
            throw new InvalidOperationException("Nie można pobrać żądania OpenId Connect.");

        await signInManager.SignOutAsync();

        return request.PostLogoutRedirectUri;
    }
}
