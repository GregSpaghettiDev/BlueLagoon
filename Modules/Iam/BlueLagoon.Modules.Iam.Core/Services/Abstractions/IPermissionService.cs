using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Pagination;

namespace BlueLagoon.Modules.Iam.Core.Services.Abstractions;

public interface IPermissionService
{
    Task<PaginatedList<PermissionDto>> GetPermissionsAsync(string searchValue, PaginationParameters paginationParameters);

    Task<PermissionDto> GetPermissionAsync(Guid permissionId);

    Task CreatePermissionAsync(string name, Guid moduleId, string description);

    Task UpdatePermissionAsync(Guid permissionId, bool? isActive, string description);

    Task DeletePermissionAsync(Guid permissionId);
}