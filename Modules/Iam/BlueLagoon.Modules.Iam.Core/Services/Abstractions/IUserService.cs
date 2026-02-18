using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Pagination;

namespace BlueLagoon.Modules.Iam.Core.Services.Abstractions;

public interface IUserService
{
    Task<PaginatedList<UserDto>> GetUsersAsync(string firstName, string lastName, string email, PaginationParameters paginationParameters, IEnumerable<Guid> roleIds = null, IEnumerable<string> permissionNames = null);

    Task<BaseUserDto> GetUserAsync(Guid id);

    Task<PaginatedList<UserRoleDto>> GetUserRolesAsync(Guid userId, PaginationParameters paginationParameters, IEnumerable<Guid> roleIds = null, IEnumerable<string> permissionNames = null, IEnumerable<string> moduleNames = null);

    Task<CreatedUserDto> CreateUserAsync(string firstName, string lastName, string phoneNumber, string email, string password, string repeatedPassword, IEnumerable<Guid> roleIds, IEnumerable<Guid> permissionIds);

    Task UpdateUserAsync(Guid userId, bool? isActive, string firstName, string lastName, string email, IList<string> requestedPermissions, IList<string> requestedRoles);
}