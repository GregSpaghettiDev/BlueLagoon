using BlueLagoon.Modules.Iam.Api.Controllers.Account.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using System.Security.Claims;

namespace BlueLagoon.Modules.Iam.Api.Controllers.Account;

[ApiController]
[Route("account")]
[Tags(IamModule.BasePath)]
[ApiExplorerSettings(IgnoreApi = true)]
internal sealed class AccountController(SignInManager<Core.DAL.Entities.User> signInManager, UserManager<Core.DAL.Entities.User> userManager) : Controller
{
    [HttpPost("login")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> Login([FromForm] LoginRequest request)
    {
        var user = await userManager.FindByNameAsync(request.UserName);
        if (user != null && await userManager.CheckPasswordAsync(user, request.Password))
        {
            var principal = await signInManager.CreateUserPrincipalAsync(user);

            if (principal.Identity is ClaimsIdentity identity)
            {
                identity.AddClaim(new Claim(OpenIddictConstants.Claims.Subject, user.Id.ToString()));
            }
            await signInManager.Context.SignInAsync(IdentityConstants.ApplicationScheme, principal, new AuthenticationProperties { IsPersistent = false });

            return LocalRedirect(request.ReturnUrl);
        }

        var encodedReturnUrl = System.Net.WebUtility.UrlEncode(request.ReturnUrl ?? "");
        return Redirect($"/account/login?error=invalid_credentials&returnUrl={encodedReturnUrl}");
    }

    [AllowAnonymous]
    [HttpGet("access-denied")]
    public IActionResult AccessDenied()
    {
        return StatusCode(403, "Brak wystarczających uprawnień do wykonania tej akcji w module IAM.");
    }
}