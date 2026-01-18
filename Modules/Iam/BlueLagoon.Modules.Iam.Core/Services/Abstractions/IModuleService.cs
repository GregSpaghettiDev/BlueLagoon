using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Pagination;

namespace BlueLagoon.Modules.Iam.Core.Services.Abstractions;

public interface IModuleService
{
    Task<PaginatedList<ModuleDto>> GetModulesAsync(string searchValue, PaginationParameters paginationParameters = null);
}
