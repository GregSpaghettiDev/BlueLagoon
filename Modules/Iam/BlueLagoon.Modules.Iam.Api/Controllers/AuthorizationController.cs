using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace BlueLagoon.Modules.Iam.Api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class AuthorizationController(IAuthorizationService authorizationService) : Controller
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
        var request = HttpContext.GetOpenIddictServerRequest();

        if (request.IsAuthorizationCodeGrantType() || request.IsRefreshTokenGrantType())
        {
            var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            return SignIn(result.Principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        return BadRequest(new OpenIddictResponse
        {
            Error = OpenIddictConstants.Errors.UnsupportedGrantType
        });
    }

    [HttpGet("~/connect/logout")]
    [HttpPost("~/connect/logout")]
    public async Task<IActionResult> Logout()
    {
        var postLogoutUri = await authorizationService.LogoutAsync();

        return SignOut(new AuthenticationProperties { RedirectUri = postLogoutUri }, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}