using BlueLagoon.Modules.Iam.Api.Controllers.Module.Requests;
using BlueLagoon.Modules.Iam.Core.Dictionaries;
using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Api;
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
internal sealed class ModuleController(IModuleService moduleService, IHttpContextAccessor httpContextAccessor)
    : BaseController(httpContextAccessor)
{
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.ReadModules)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ModuleDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(GetModulesAsync), Summary = "Pobranie listy modułów", Description = "Zwraca moduły zarejestrowane w IAM")]
    public async Task<IActionResult> GetModulesAsync([FromQuery] ModuleListRequest request)
    {
        var result = await moduleService.GetModulesAsync(request.SearchValue, request.PaginationParameters);

        return ContentResult(result, HttpStatusCode.OK);
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.AddOrDeleteModule)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(CreateModuleAsync), Summary = "Dodanie modułu do rejestru IAM", Description = "Dodaje wskazany moduł do rejestru IAM")]
    public async Task<IActionResult> CreateModuleAsync([FromBody] CreateModuleRequest request)
    {
        await moduleService.AddModuleAsync(request.Name, request.Description, request.OpenApiUri, request.BaseUrl);

        return CreatedContentResult();
    }

    [HttpPatch("{moduleId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.UpdateModule)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [SwaggerOperation(OperationId = nameof(UpdateModuleAsync),
        Summary = "Zmienia wartości poszczególnych pól modułu.",
        Description = "Do zmiany poszczególnych wartości należy w żądaniu zdefiniować odpowiednie pola i ich wartości (IsActive, Name, BaseUrl, OpenApiPath, IsActive). " +
        "Dezaktywacja modułu wiąże się z dezaktywacją endpointów i uprawnień. Uprawnienia przypisane do ról i/lub użytkowników sa trwale z nich usuwane.")]
    public async Task<IActionResult> UpdateModuleAsync([FromRoute] Guid moduleId, [FromBody] UpdateModuleRequest request)
    {
        await moduleService.UpdateModuleAsync(moduleId, request.Name, request.BaseUrl, request.OpenApiPath, request.IsActive);

        return NoContentResult();
    }

    [HttpDelete("{moduleId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.AddOrDeleteModule)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [SwaggerOperation(OperationId = nameof(DeleteModuleAsync), 
        Summary = "Trwałe usunięcie modułu.", 
        Description = "Usuwa moduł razem z zarejestrowanymi ednpointami i zdefiniowanymi do nich uprawnieniami. Uprawnienia przypisane do ról i/lub użytkowników sa trwale z nich usuwane.")]
    public async Task<IActionResult> DeleteModuleAsync([FromRoute] Guid moduleId)
    {
        await moduleService.DeleteModuleAsync(moduleId);

        return NoContentResult();
    }
}