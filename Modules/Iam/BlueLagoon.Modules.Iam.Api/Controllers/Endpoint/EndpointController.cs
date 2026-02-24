using BlueLagoon.Modules.Iam.Api.Controllers.Endpoint.Requests;
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

namespace BlueLagoon.Modules.Iam.Api.Controllers.Endpoint;


[ApiController]
[Tags(IamModule.BasePath)]
[Route(IamModule.BasePath + "/endpoints")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = AuthorizationPolicies.IamScopeRequirement)]
internal sealed class EndpointController(IHttpContextAccessor httpContextAccessor, IEndpointService service) 
    : BaseController(httpContextAccessor)
{
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.ReadEndpoint)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PaginatedList<RegisteredEndpointDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [SwaggerOperation(
        OperationId = nameof(GetEndpointsAsync), 
        Summary = "Pobranie listy zarejestrowanych endpointów", 
        Description = "Zwraca listę zarejestrowanych endpointów pochodzących z modułów/serwisów")]
    public async Task<IActionResult> GetEndpointsAsync([FromQuery] EdnpointListRequest request)
    {
        var result = await service.GetEndpointsAsync(request.ModuleName,
                                                     request.HttpMethod,
                                                     request.Path,
                                                     request.WithoutPermissionMappings,
                                                     request.PaginationParameters);

        return ContentResult(result, HttpStatusCode.OK);
    }

    [HttpGet("{endpointId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.ReadEndpoint)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(RegisteredEndpointDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [SwaggerOperation(
        OperationId = nameof(GetEndpointAsync), 
        Summary = "Pobranie szczgółowych informacji o zarejestrowanym endpoincie.", 
        Description = "Zwraca informacje dla wskazanego endpointu wraz z przypisyanymi restrykcjami.")]
    public async Task<IActionResult> GetEndpointAsync([FromRoute] Guid endpointId)
    {
        var result = await service.GetEndpointAsync(endpointId);

        return ContentResult(result, HttpStatusCode.OK);
    }

    [HttpGet("{endpointId:guid}/security-mappings")]
    [Authorize(Policy = AuthorizationPolicies.ReadEndpoint)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(EndpointMappingsDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [SwaggerOperation(
        OperationId = nameof(GetEndpointSecurityMappingsAsync),
        Summary = "Pobranie informacji o zmapowanych wymaganiach dostępowych oraz dostępnych do przypisania.",
        Description = "Zwraca dwa zbiory zawierające przypisane wymagania dostępowe oraz dostępne do przypisania w ramach wskazanego endpointa.")]
    public async Task<IActionResult> GetEndpointSecurityMappingsAsync([FromRoute] Guid endpointId, EndpointMappingsRequest request)
    {
        var result = await service.GetEndpointSecurityMappingsAsync(endpointId,
                                                                    request.PermissionIds,
                                                                    request.FilterAvailable,
                                                                    request.FilterAssigned,
                                                                    request.PaginationParameters);

        return ContentResult(result, HttpStatusCode.OK);
    }

    [HttpPatch("{endpointId:guid}/security-mappings")]
    [Authorize(Policy = AuthorizationPolicies.ReadEndpoint)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [SwaggerOperation(
        OperationId = nameof(UpdateEndpoinSecurityMappingsAsync),
        Summary = "Aktualizuje wymagania dostępowe w ramach wskazanego endpointa.",
        Description = "Należy wskazać listę wymagań, które mają być przypisane do danego endpointa. Dodawane są tylko te wymagania, które jeszcze nie są realnie przypisane w bazie.")]
    public async Task<IActionResult> UpdateEndpoinSecurityMappingsAsync([FromRoute] Guid endpointId, UpdateEndpointSecurityMappingsRequest request)
    {
        await service.UpdateEndpointRestrictionsAsync(endpointId, request.RequirementIds);

        return NoContentResult();
    }
}