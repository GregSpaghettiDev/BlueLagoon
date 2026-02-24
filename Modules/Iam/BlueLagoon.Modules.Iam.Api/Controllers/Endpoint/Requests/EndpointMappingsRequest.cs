using BlueLagoon.Shared.DevTools.Pagination;

namespace BlueLagoon.Modules.Iam.Api.Controllers.Endpoint.Requests;

public sealed class EndpointMappingsRequest
{
    public IList<Guid> PermissionIds { get; set; }

    public bool FilterAvailable { get; set; }

    public bool FilterAssigned { get; set; }

    public PaginationParameters PaginationParameters { get; set; }
}