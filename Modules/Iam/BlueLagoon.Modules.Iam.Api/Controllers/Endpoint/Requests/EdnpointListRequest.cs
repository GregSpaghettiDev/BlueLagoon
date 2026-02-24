using BlueLagoon.Shared.DevTools.Pagination;

namespace BlueLagoon.Modules.Iam.Api.Controllers.Endpoint.Requests;

public sealed class EdnpointListRequest
{
    public string ModuleName { get; set; }

    public string HttpMethod { get; set; }

    public string Path { get; set; }

    public bool WithoutPermissionMappings { get; set; }

    public PaginationParameters PaginationParameters { get; set; }
}