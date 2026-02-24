using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Pagination;

namespace BlueLagoon.Modules.Iam.Core.Services.Abstractions;

public interface IEndpointService
{
    Task<PaginatedList<RegisteredEndpointDto>> GetEndpointsAsync(string moduleName, string httpMethod, string path, bool withoudPermissionMappings, PaginationParameters paginationParameters);

    Task<RegisteredEndpointDto> GetEndpointAsync(Guid endpointId);

    Task<EndpointMappingsDto> GetEndpointSecurityMappingsAsync(Guid endpointId, IList<Guid> permissionIds, bool filterAvailable, bool filterAssigned, PaginationParameters paginationParameters);

    Task UpdateEndpointRestrictionsAsync(Guid endpointId, IList<Guid> requirementIds);
}