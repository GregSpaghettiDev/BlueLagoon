using BlueLagoon.Modules.Iam.Api.Controllers.Permission.Requests;
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

namespace BlueLagoon.Modules.Iam.Api.Controllers.Permission;

[ApiController]
[Tags(IamModule.BasePath)]
[Route(IamModule.BasePath + "/permissions")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = AuthorizationPolicies.IamScopeRequirement)]
internal sealed class ModuleController(IPermissionService permissionService, IHttpContextAccessor httpContextAccessor)
    : BaseController(httpContextAccessor)
{
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.ReadPermission)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PaginatedList<PermissionDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [SwaggerOperation(OperationId = nameof(GetPermissionsAsync), Summary = "Pobranie listy zdefiniowanych uprawnień", Description = "Zwraca listę uprawnień z możliwością filtrowania po dostępnych parametrach.")]
    public async Task<IActionResult> GetPermissionsAsync([FromQuery] PermissionListRequest request)
    {
        var result = await permissionService.GetPermissionsAsync(request.SearchValue, request.PaginationParameters);

        return ContentResult(result, HttpStatusCode.OK);
    }

    [HttpGet("{permissionId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.ReadPermission)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(RoleWithPermissionsDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [SwaggerOperation(OperationId = nameof(GetPermissionAsync), Summary = "Pobranie informacji o uprawnieniu", Description = "Zwraca inforacje dotyczące konkretnego uprawnienia")]
    public async Task<IActionResult> GetPermissionAsync([FromRoute] Guid permissionId)
    {
        var result = await permissionService.GetPermissionAsync(permissionId);

        return ContentResult(result, HttpStatusCode.OK);
    }

    [HttpPost()]
    [Authorize(Policy = AuthorizationPolicies.AddOrDeletePermission)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [SwaggerOperation(OperationId = nameof(CreatePermissionAsync), 
        Summary = "Dodanie nowego uprawnienia", 
        Description = "Tworzy nowe uprawnienie w kontekście danego modułu. Posiadanie danego uprawnienia będzie definiowało czy dany użytkownik posiada scope dla danego modułu.")]
    public async Task<IActionResult> CreatePermissionAsync([FromBody] CreatePermissionRequest request)
    {
        await permissionService.CreatePermissionAsync(request.Name, request.ModuleId, request.Description);

        return CreatedContentResult();
    }

    [HttpPatch("{permissionId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.UpdatePermission)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(UpdatePermissionAsync),
        Summary = "Zmienia wartości poszczególnych pól uprawnienia.",
        Description = "Do zmiany poszczególnych wartości należy w żądaniu zdefiniować odpowiednie pola i ich wartości (IsActive, Description)." +
        "Dezaktywacja uprawnienia wiąże się z dezaktywacją przypisanych uprawnień do użytkownika i/lub roli.")]
    public async Task<IActionResult> UpdatePermissionAsync([FromRoute] Guid permissionId, [FromBody] UpdatePermissionRequest request)
    {
        await permissionService.UpdatePermissionAsync(permissionId, request.IsActive, request.Description);

        return NoContentResult();
    }

    [HttpDelete("{permissionId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.AddOrDeletePermission)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [SwaggerOperation(OperationId = nameof(DeletRoleAsync),
        Summary = "Trwałe usunięcie uprawnienia.",
        Description = "Usuwa uprawnienie i ich przypisanie do ról i użytkowników.")]
    public async Task<IActionResult> DeletRoleAsync([FromRoute] Guid permissionId)
    {
        await permissionService.DeletePermissionAsync(permissionId);

        return NoContentResult();
    }
}