using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Modules.Iam.Core.Services.Dto;
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

        if (!httpContext.User.Identity?.IsAuthenticated ?? true)
            return new AuthorizationResultDto { IsChallenge = true };

        var user = await userManager.GetUserAsync(httpContext.User);
        var principal = await signInManager.CreateUserPrincipalAsync(user);

        var userAllowedScopes = await GetAuthorizedModuleScopesAsync(user, principal);
        principal.SetScopes(userAllowedScopes);


        return new AuthorizationResultDto
        {
            IsChallenge = false,
            Principal = principal
        };
    }

    public async Task<IEnumerable<string>> GetAuthorizedModuleScopesAsync(User user, ClaimsPrincipal principal)
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
}
