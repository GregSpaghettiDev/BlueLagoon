using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Pagination;

namespace BlueLagoon.Modules.Iam.Core.Services.Abstractions;

public interface IRoleService
{
    Task<PaginatedList<RoleDto>> GetRolesAsync(string searchValue, PaginationParameters paginationParameters);

    Task<RoleWithPermissionsDto> GetRoleAsync(Guid roleId);

    Task UpdateRoleAsync(Guid roleId, IList<Guid> permissionIds, bool? IsActive);

    Task CreateRoleAsync(ValueObjects.Role role, IList<Guid> permissionIds);

    Task DeleteRoleAsync(Guid roleId);
}