using BlueLagoon.Modules.Iam.Api.Controllers.Role.Requests;
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

namespace BlueLagoon.Modules.Iam.Api.Controllers.Role;

[ApiController]
[Tags(IamModule.BasePath)]
[Route(IamModule.BasePath + "/roles")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = AuthorizationPolicies.IamScopeRequirement)]
internal sealed class RoleController(IHttpContextAccessor httpContextAccessor, IRoleService roleService) : BaseController(httpContextAccessor)
{
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.ReadRoles)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PaginatedList<RoleDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(GetRolesAsync), Summary = "Pobranie listy zdefiniowanych ról", Description = "Zwraca role zdefiniowane w module IAM")]
    public async Task<IActionResult> GetRolesAsync([FromQuery] RoleListRequest request)
    {
        var result = await roleService.GetRolesAsync(request.SearchValue, request.PaginationParameters);

        return ContentResult(result, HttpStatusCode.OK);
    }

    [HttpGet("{roleId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.ReadRoles)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(RoleWithPermissionsDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(GetRolesAsync), Summary = "Pobranie listy zdefiniowanych ról", Description = "Zwraca role zdefiniowane w module IAM")]
    public async Task<IActionResult> GetRoleAsync([FromRoute] Guid roleId)
    {
        var result = await roleService.GetRoleAsync(roleId);

        return ContentResult(result, HttpStatusCode.OK);
    }

    [HttpPost()]
    [Authorize(Policy = AuthorizationPolicies.AddOrDeleteRole)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(CreateRoleAsync), Summary = "Utworzenie nowej roli", Description = "Tworzy nową rolę")]
    public async Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleRequest request)
    {
        await roleService.CreateRoleAsync(new Core.ValueObjects.Role(Guid.NewGuid(), request.Code, request.Name), request.RoleIds);

        return CreatedContentResult();
    }

    [HttpPatch("{roleId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.UpdateRole)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [SwaggerOperation(OperationId = nameof(UpdateRoleAsync),
        Summary = "Zmienia wartości poszczególnych pól roli takich jak stan aktywny/nieaktywny czy przypisane uprawnienia ",
        Description = "Do zmiany poszczególnych wartości należy w żądaniu zdefiniować odpowiednie pola i ich wartości (IsActive, PermissionIds)." +
        "Dezaktywacja roliu wiąże się z dezaktywacją przypisanych uprawnień. Nie można jednocześnie przypisać uprawnień i dezaktywować roli.")]
    public async Task<IActionResult> UpdateRoleAsync([FromRoute] Guid roleId, [FromBody] UpdateRoleRequest request)
    {
        await roleService.UpdateRoleAsync(roleId, request.PermissionIds, request.IsActive);

        return NoContentResult();
    }

    [HttpDelete("{roleId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.AddOrDeleteRole)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [SwaggerOperation(OperationId = nameof(DeletRoleAsync),
        Summary = "Trwałe usunięcie modułu.",
        Description = "Usuwa moduł razem z zarejestrowanymi ednpointami i zdefiniowanymi do nich uprawnieniami. Uprawnienia przypisane do ról i/lub użytkowników sa trwale z nich usuwane.")]
    public async Task<IActionResult> DeletRoleAsync([FromRoute] Guid moduleId)
    {
        await roleService.DeleteRoleAsync(moduleId);

        return NoContentResult();
    }
}