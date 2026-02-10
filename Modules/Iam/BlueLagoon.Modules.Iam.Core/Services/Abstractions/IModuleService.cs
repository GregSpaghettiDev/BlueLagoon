using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Pagination;

namespace BlueLagoon.Modules.Iam.Core.Services.Abstractions;

public interface IModuleService
{
    Task<PaginatedList<ModuleDto>> GetModulesAsync(string searchValue, PaginationParameters paginationParameters = null);

    internal Task<IEnumerable<EndpointDefinitionDto>> GetEndpointDefinitionsFromOpenApi(string uri, string moduleCode);

    Task AddModuleAsync(string name, string description, string openApiUri, string baseUrl);

    Task UpdateModuleAsync(Guid moduleId, string name, string baseUrl, string openApiPath, bool? isActive);
}