using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Pagination;

namespace BlueLagoon.Modules.Iam.Core.Services.Abstractions;

public interface IRoleService
{
    Task<PaginatedList<RoleDto>> GetRolesAsync(string searchValue, PaginationParameters paginationParameters);

    Task<RoleWithPermissionsDto> GetRoleAsync(Guid roleId);

    Task UpdateRolePermissionsAsync(Guid roleId, IList<Guid> permissionIds);
}