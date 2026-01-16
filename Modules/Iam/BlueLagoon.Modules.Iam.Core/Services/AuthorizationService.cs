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
                                           UserManager<User> userManager,
                                           IOpenIddictApplicationManager applicationManager,
                                           IOpenIddictScopeManager openIddictScopeManager) 
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

        var userAllowedScopes = new List<string>();

        //openIddictScopeManager.
        //if (/* Sprawdź w DB czy user ma dostęp do Utrzymania */) userAllowedScopes.Add("retention");
        //if (/* Sprawdź w DB czy user ma dostęp do Motywacji */) userAllowedScopes.Add("motivation");

        return Task.FromResult(new AuthorizationResultDto
        {
            IsChallenge = false,
            Principal = principal
        }).Result;
    }
}
