using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using IAuthorizationService = BlueLagoon.Modules.Iam.Core.Services.Abstractions.IAuthorizationService;

namespace BlueLagoon.Modules.Iam.Api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
internal class AuthorizationController(IAuthorizationService authorizationService) : Controller
{
    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Authorize()
    {
        var result = await authorizationService.AuthorizeAsync();

        if (result.IsChallenge)
            return Challenge(new AuthenticationProperties { RedirectUri = result.RedirectUri }, IdentityConstants.ApplicationScheme);

        return SignIn(result.Principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [HttpPost("~/connect/token")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Exchange()
    {
        var exchangeResult = await authorizationService.ExchangeAuthorizationCodeForTokensOrRefreshSessionAsync();

        if ((exchangeResult?.ExchangeResult?.Succeeded ?? true) == false && exchangeResult.IsRefreshTokenGrant)
            return Forbid(
                new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "Sesja użytkownika wygasła lub token jest nieważny."
                }),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        if (exchangeResult?.ExchangeResult?.Succeeded ?? false)
            return SignIn(exchangeResult.ExchangeResult.Principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        return BadRequest(new OpenIddictResponse
        {
            Error = OpenIddictConstants.Errors.UnsupportedGrantType,
            ErrorDescription = "Nieobsługiwany typ żądania podczas próby wymiany tokenów."
        });
    }

    [AllowAnonymous]
    [HttpGet("~/connect/logout")]
    [HttpPost("~/connect/logout")]
    public async Task<IActionResult> Logout()
    {
        var postLogoutUri = await authorizationService.LogoutAsync();

        return SignOut(new AuthenticationProperties { RedirectUri = postLogoutUri }, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}