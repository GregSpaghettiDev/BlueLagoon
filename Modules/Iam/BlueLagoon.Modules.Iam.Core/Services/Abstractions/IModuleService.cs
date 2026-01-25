using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Base;
using BlueLagoon.Shared.DevTools.Pagination;

namespace BlueLagoon.Modules.Iam.Core.Services.Abstractions;

public interface IModuleService
{
    Task<PaginatedList<ModuleDto>> GetModulesAsync(string searchValue, PaginationParameters paginationParameters = null);

    Task<IEnumerable<EndpointDefinitionDto>> GetEndpointDefinitionsFromOpenApi(string uri, string moduleCode);

    Task AddModuleAsync(string name, string description, string openApiUri, string baseUrl);

    Task SetOpenApiPathAsync(BaseId moduleId, string path);

    Task SetOpenApiUrlAsync(BaseId moduleId, string url);

    Task SetOpenApiUrlAndOrPathAsync(BaseId moduleId, string url = null, string path = null);

    Task DeactivateModuleAsync(BaseId moduleId);

    Task ActivateModuleAsync(BaseId moduleId);

    Task ChangeNameAsync(BaseId moduleId, string newName);
}