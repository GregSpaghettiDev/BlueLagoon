using BlueLagoon.Shared.DevTools.Api;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace BlueLagoon.Modules.Iam.Api.Controllers;


[ApiController]
[Route("modules")]
public sealed class ModuleController : BaseController
{
    public ModuleController()
    {
    }

    [HttpGet("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(GetModules))]
    public async Task<IActionResult> GetModules(Guid id) 
    {
        return ContentResult(new ModuleController(), HttpStatusCode.OK);
    }
}
