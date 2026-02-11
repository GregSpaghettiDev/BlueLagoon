using BlueLagoon.Shared.DevTools.Pagination;

namespace BlueLagoon.Modules.Iam.Api.Controllers.Module.Requests;

public sealed class ModuleListRequest
{
    public string SearchValue { get; set; }

    public PaginationParameters PaginationParameters { get; set; }
}
