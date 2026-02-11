using BlueLagoon.Shared.DevTools.Pagination;

namespace BlueLagoon.Modules.Iam.Api.Controllers.Permission.Requests;

public sealed class PermissionListRequest
{
    public string SearchValue { get; set;  }

    public IEnumerable<Guid> ModuleIds { get; set; }

    public PaginationParameters PaginationParameters { get; set; }
}
