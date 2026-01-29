using BlueLagoon.Modules.Iam.Api.Controllers.Account.Requests;
using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlueLagoon.Modules.Iam.Api.Controllers.Account;

[ApiController]
[Route("account")]
internal class AccountController(SignInManager<User> signInManager) : Controller // Używamy Controller zamiast ControllerBase, by móc zwracać widoki
{
    // POST: /login
    // To tutaj trafiają dane z formularza (login/hasło)
    [HttpPost("login")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> Login([FromForm] LoginRequest request)
    {
        // 1. Logowanie przez SignInManager (to wystawi ciasteczko)
        var result = await signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);

        if (result.Succeeded)
        {
            // 2. Sukces! Wracamy pod adres, który był w ReturnUrl 
            // (czyli z powrotem do Twojego ~/connect/authorize)
            return LocalRedirect(request.ReturnUrl);
        }

        var encodedReturnUrl = System.Net.WebUtility.UrlEncode(request.ReturnUrl ?? "");
        return Redirect($"/login?error=invalid_credentials&returnUrl={encodedReturnUrl}");
    }

    [HttpGet("access-denied")]
    public IActionResult AccessDenied()
    {
        // Możesz zwrócić widok HTML z ładnym komunikatem
        // lub po prostu wynik JSON, jeśli to API
        return StatusCode(403, "Brak wystarczających uprawnień do wykonania tej akcji w module IAM.");
    }
}