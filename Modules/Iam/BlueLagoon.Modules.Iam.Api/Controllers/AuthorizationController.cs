using Microsoft.AspNetCore.Mvc;

namespace BlueLagoon.Modules.Iam.Api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class AuthorizationController : Controller
{
    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    [IgnoreAntiforgeryToken]
    public Task<IActionResult> Authorize()
    {
        var returnUrl = Request.PathBase + Request.Path + Request.QueryString;

        return null;
    }
}
