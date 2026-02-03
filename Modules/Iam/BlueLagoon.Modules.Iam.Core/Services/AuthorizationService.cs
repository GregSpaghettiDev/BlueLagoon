using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
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
        var request = GetOpenIddictRequest();

        var returnUrl = httpContext.Request.PathBase + httpContext.Request.Path + httpContext.Request.QueryString;

        if (!httpContext.User.Identity?.IsAuthenticated ?? true)
            return new AuthorizationResultDto
            {
                IsChallenge = true,
                RedirectUri = returnUrl
            };

        var userId = httpContext.GetUserId(); 
        if (string.IsNullOrWhiteSpace(userId))
            await LogoutAsync();

        var user = await userManager.FindByIdAsync(userId);
        var principal = await signInManager.CreateUserPrincipalAsync(user);
        principal.SetClaim(OpenIddictConstants.Claims.Subject, userId);

        var scopesRequestedByApplication = request.GetScopes();
        var userAllowedScopes = await GetAuthorizedModuleScopesAsync(principal);
        var finalScopes = new List<string>()
        {
            OpenIddictConstants.Scopes.OpenId,
            OpenIddictConstants.Scopes.Profile,
            OpenIddictConstants.Scopes.OfflineAccess
        };
        finalScopes.AddRange(scopesRequestedByApplication.Intersect(userAllowedScopes));
        principal.SetScopes(finalScopes);

        foreach (var claim in principal.Claims)
        {
            var destinations = new List<string> { OpenIddictConstants.Destinations.AccessToken };

            if (claim.Type is OpenIddictConstants.Claims.Name or OpenIddictConstants.Claims.Role)
                destinations.Add(OpenIddictConstants.Destinations.IdentityToken);

            claim.SetDestinations(destinations);
        }

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
            authorizedModules.Add("iam");


        return authorizedModules;
    }

    public async Task<string> LogoutAsync()
    {
        var request = GetOpenIddictRequest();

        await signInManager.SignOutAsync();

        return request.PostLogoutRedirectUri;
    }

    public async Task<ExchangeResultDto> ExchangeAuthorizationCodeForTokensOrRefreshSessionAsync()
    {
        var request = GetOpenIddictRequest();

        if (request.IsRefreshTokenGrantType())
        {
            var identityResult = await httpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
            if (!identityResult.Succeeded)
                return new ExchangeResultDto 
                { 
                    ExchangeResult = identityResult,
                    IsRefreshTokenGrant = true
                };

            return new ExchangeResultDto
            {
                ExchangeResult = await httpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme),
                IsRefreshTokenGrant = true
            };
        }

        if (request.IsAuthorizationCodeGrantType())
            return new ExchangeResultDto
            {
                ExchangeResult = await httpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme),
                IsRefreshTokenGrant = false
            };

        return new ExchangeResultDto
        {
            IsRefreshTokenGrant = false
        };
    }
    
    private OpenIddictRequest GetOpenIddictRequest()
    {
        var request = httpContext.GetOpenIddictServerRequest();

        if (request is null)
            throw new InvalidOperationException("Nie można pobrać żądania OpenId Connect.");

        return request;
    }
}