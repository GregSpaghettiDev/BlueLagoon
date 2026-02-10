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
    public async Task<IActionResult> GetRolesAsync([FromQuery] string SearchValue, [FromQuery] PaginationParameters PaginationParameters)
    {
        var result = await roleService.GetRolesAsync(SearchValue, PaginationParameters);

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
    public async Task<IActionResult> GetRoleAsync([FromRoute] Guid RoleId)
    {
        var result = await roleService.GetRoleAsync(RoleId);

        return ContentResult(result, HttpStatusCode.OK);
    }

    [HttpPost()]
    [Authorize(Policy = AuthorizationPolicies.AddRole)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(CreateRoleAsync), Summary = "Utworzenie nowej roli", Description = "Tworzy nową rolę")]
    public async Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleRequest Request)
    {
        await roleService.CreateRoleAsync(new Core.ValueObjects.Role(Guid.NewGuid(), Request.Code, Request.Name), Request.RoleIds);

        return CreatedContentResult();
    }

    [HttpPatch("{roleId:guid}/activate")]
    [Authorize(Policy = AuthorizationPolicies.UpdateRole)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(UpdateRoleAsync), 
        Summary = "Zmienia wartości poszczególnych pól roli takich jak stan aktywny/nieaktywny czy przypisane uprawnienia ", 
        Description = "Do zmiany poszczególnych wartości należy w żądaniu zdefiniować odpowiednie pola i ich wartości (IsActive, PermissionIds).")]
    public async Task<IActionResult> UpdateRoleAsync([FromRoute] Guid RoleId, [FromBody] UpdateRoleRequest Request)
    {
        await roleService.UpdateRoleAsync(RoleId, Request.PermissionIds, Request.IsActive);

        return NoContentResult();
    }
}