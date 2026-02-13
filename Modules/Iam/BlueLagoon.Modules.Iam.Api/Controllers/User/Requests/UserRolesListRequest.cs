using BlueLagoon.Shared.DevTools.Pagination;

namespace BlueLagoon.Modules.Iam.Api.Controllers.User.Requests;

public sealed class UserRolesListRequest
{
    public PaginationParameters PaginationParameters { get; set; }

    public IEnumerable<Guid> RoleIds { get; set; }

    public IEnumerable<string> PermissionNames {  get; set; }

    public IEnumerable<string> ModuleNames { get; set; }
}
