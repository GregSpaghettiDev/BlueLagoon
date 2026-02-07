using BlueLagoon.Modules.Iam.Api.Controllers.Module.Requests;
using BlueLagoon.Modules.Iam.Core.Dictionaries;
using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Api;
using BlueLagoon.Shared.DevTools.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace BlueLagoon.Modules.Iam.Api.Controllers.Module;

[ApiController]
[Tags(IamModule.BasePath)]
[Route(IamModule.BasePath + "/modules")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = AuthorizationPolicies.IamScopeRequirement)]
public sealed class ModuleController(IModuleService moduleService, IHttpContextAccessor httpContextAccessor)
    : BaseController(httpContextAccessor)
{
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.ListModules)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ModuleDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(GetModulesAsync), Summary = "Pobranie listy modułów", Description = "Zwraca moduły zarejestrowane w IAM")]
    public async Task<IActionResult> GetModulesAsync([FromQuery] string SearchValue, [FromQuery] PaginationParameters PaginationParameters)
    {
        var result = await moduleService.GetModulesAsync(SearchValue, PaginationParameters);

        return ContentResult(result, HttpStatusCode.OK);
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.AddModule)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(CreateModuleAsync), Summary = "Dodanie modułu do rejestru IAM", Description = "Dodaje wskazany moduł do rejestru IAM")]
    public async Task<IActionResult> CreateModuleAsync([FromBody] CreateModuleRequest request)
    {
        await moduleService.AddModuleAsync(request.Name, request.Description, request.OpenApiUri, request.BaseUrl);

        return CreatedContentResult();
    }

    [HttpPut("{moduleId:guid}/open-api-path")]
    [Authorize(Policy = AuthorizationPolicies.UpdateModule)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(UpdateOpenApiPathAsync), Summary = "Zaktualizowanie ścieżki do dokumentacji API", Description = "Ustawia nową wartość dla ścieżki wskazującej na dokumentację OpenApi")]
    public async Task<IActionResult> UpdateOpenApiPathAsync([FromRoute] Guid ModuleId, [FromBody] UpdateOpenApiPathRequest request)
    {
        await moduleService.SetOpenApiPathAsync(ModuleId, request.Path);

        return NoContentResult();
    }

    [HttpPut("{moduleId:guid}/open-api-url")]
    [Authorize(Policy = AuthorizationPolicies.UpdateModule)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(UpdateOpenApiUrlAsync), Summary = "Zaktualizowanie url do serwera dokumentacji API", Description = "Ustawia nową wartość dla url wskazującego na serwer dokumentacji OpenApi")]
    public async Task<IActionResult> UpdateOpenApiUrlAsync([FromRoute] Guid ModuleId, [FromBody] UpdateOpenApiUrlRequest request)
    {
        await moduleService.SetOpenApiUrlAsync(ModuleId, request.Url);

        return NoContentResult();
    }

    [HttpPut("{moduleId:guid}/deactivated")]
    [Authorize(Policy = AuthorizationPolicies.UpdateModule)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(DeactivateModuleAsync), Summary = "Dezaktywacja modułu w systemie", Description = "Ustawia moduł w stan zdezaktywowany")]
    public async Task<IActionResult> DeactivateModuleAsync([FromRoute] Guid ModuleId)
    {
        await moduleService.DeactivateModuleAsync(ModuleId);

        return NoContentResult();
    }

    [HttpPut("{moduleId:guid}/activated")]
    [Authorize(Policy = AuthorizationPolicies.UpdateModule)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(ActivateModuleAsync), Summary = "Aktywacja modułu w systemie", Description = "Ustawia moduł w stan aktywny")]
    public async Task<IActionResult> ActivateModuleAsync([FromRoute] Guid ModuleId)
    {
        await moduleService.ActivateModuleAsync(ModuleId);

        return NoContentResult();
    }
}