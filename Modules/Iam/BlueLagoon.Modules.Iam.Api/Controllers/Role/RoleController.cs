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
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(GetRolesAsync), Summary = "Pobranie listy zdefiniowanych ról", Description = "Zwraca role zdefiniowane w module IAM")]
    public async Task<IActionResult> GetRoleAsync([FromRoute] Guid RoleId)
    {
        var result = await roleService.GetRoleAsync(RoleId);

        return ContentResult(result, HttpStatusCode.OK);
    }

    [HttpGet("{roleId:guid}/permissions")]
    [Authorize(Policy = AuthorizationPolicies.ReadRoles)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [SwaggerOperation(OperationId = nameof(UpdateRolePermissionsAsync), Summary = "Pobranie listy zdefiniowanych ról", Description = "Zwraca role zdefiniowane w module IAM")]
    public async Task<IActionResult> UpdateRolePermissionsAsync([FromRoute] Guid RoleId, [FromBody] UpdateRolePermissionsRequest Request)
    {
        await roleService.UpdateRolePermissionsAsync(RoleId, Request.PermissionIdsToReplace);

        return NoContentResult();
    }
}