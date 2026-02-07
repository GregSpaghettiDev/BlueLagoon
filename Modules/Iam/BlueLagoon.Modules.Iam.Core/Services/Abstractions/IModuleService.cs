using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Pagination;

namespace BlueLagoon.Modules.Iam.Core.Services.Abstractions;

public interface IModuleService
{
    Task<PaginatedList<ModuleDto>> GetModulesAsync(string searchValue, PaginationParameters paginationParameters = null);

    Task<IEnumerable<EndpointDefinitionDto>> GetEndpointDefinitionsFromOpenApi(string uri, string moduleCode);

    Task AddModuleAsync(string name, string description, string openApiUri, string baseUrl);

    Task SetOpenApiPathAsync(Guid moduleId, string path);

    Task SetOpenApiUrlAsync(Guid moduleId, string url);

    Task SetOpenApiUrlAndOrPathAsync(Guid moduleId, string url = null, string path = null);

    Task DeactivateModuleAsync(Guid moduleId);

    Task ActivateModuleAsync(Guid moduleId);

    Task ChangeNameAsync(Guid moduleId, string newName);
}