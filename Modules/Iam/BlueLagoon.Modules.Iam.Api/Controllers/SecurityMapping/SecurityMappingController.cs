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

namespace BlueLagoon.Modules.Iam.Api.Controllers.SecurityMapping;


[ApiController]
[Tags(IamModule.BasePath)]
[Route(IamModule.BasePath + "/security-mappings")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = AuthorizationPolicies.IamScopeRequirement)]
internal sealed class SecurityMappingController(IHttpContextAccessor httpContextAccessor, ISecurityMappingService service) 
    : BaseController(httpContextAccessor)
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
}
