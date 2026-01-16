using Microsoft.AspNetCore.Mvc;

namespace BlueLagoon.Modules.Iam.Api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class AuthorizationController : Controller
{
    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    public Task<IActionResult> Authorize()
    {
        return null;
    }
}
