using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Api;
using BlueLagoon.Shared.DevTools.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
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

    //[HttpPost()]
    //[Consumes(MediaTypeNames.Application.Json)]
    //[ProducesResponseType((int)HttpStatusCode.NoContent)]
    //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
    //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    //[ProducesResponseType((int)HttpStatusCode.Forbidden)]
    //[SwaggerOperation(OperationId = nameof(CreateModule), Summary = "Dodanie modułu do rejestru IAM", Description = "Dodaje wskazany moduł do rejestru IAM")]
    //public async Task<IActionResult> CreateModule([FromBody] string Name, string Description, string OpenApiUri, string BaseUrl)
    //{
    //    var result = await moduleService.CreateModuleAsync(PaginationParameters);

    //    return ContentResult(result, HttpStatusCode.OK);

    //    if (await scopeManager.FindByNameAsync(Scope.Iam.Name) == null)
    //        await scopeManager.CreateAsync(new OpenIddictScopeDescriptor { Name = Scope.Iam.Name, DisplayName = Scope.Iam.Description });
    //}
}
