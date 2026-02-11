using BlueLagoon.Shared.DevTools.Pagination;

namespace BlueLagoon.Modules.Iam.Api.Controllers.Role.Requests;

public sealed class RoleListRequest
{
    public string SearchValue { get; set; }

    public PaginationParameters PaginationParameters { get; set; }
}
