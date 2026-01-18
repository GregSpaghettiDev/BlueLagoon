using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Api;
using BlueLagoon.Shared.DevTools.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace BlueLagoon.Modules.Iam.Api.Controllers;

[Tags(IamModule.BasePath)]
[ApiController]
[Route("modules")]
public sealed class ModuleController(IModuleService moduleService) : BaseController
{
    [HttpGet]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ModuleDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(GetModules), Summary = "Pobranie listy modułów", Description = "Zwraca moduły zarejestrowane w IAM")]
    public async Task<IActionResult> GetModules([FromQuery] string SearchValue, [FromQuery] PaginationParameters PaginationParameters) 
    {
        var result = await moduleService.GetModulesAsync(SearchValue, PaginationParameters);

        return ContentResult(result, HttpStatusCode.OK);
    }

    //[HttpPost("discovery")]
    //[Consumes(MediaTypeNames.Application.Json)]
    //[ProducesResponseType(typeof(DiscoveredModuleDto), (int)HttpStatusCode.OK)]
    //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
    //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    //[ProducesResponseType((int)HttpStatusCode.Forbidden)]
    //[SwaggerOperation(OperationId = nameof(DiscoverAvailableModules))]
    //public async Task<IActionResult> DiscoverAvailableModules([FromBody] PaginationParameters PaginationParameters)
    //{
    //    var result = await moduleService.DiscoverAvailableModules(PaginationParameters);

    //    return ContentResult(result, HttpStatusCode.OK);
    //}
}
