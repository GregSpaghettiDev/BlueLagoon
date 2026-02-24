using BlueLagoon.Shared.DevTools.Pagination;

namespace BlueLagoon.Modules.Iam.Core.Services.Dto;

public class EndpointRequirementDto
{
    public Guid? RegisteredEndpointId { get; set; }

    public Guid PermissionId { get; set; }

    public Guid CreatorId { get; set; }

    public string CreatorName { get; set; }

    public Guid? ModificatorId { get; set; }

    public string ModificatorName { get; set; }

    public string Code { set; get; }

    public string Type { get; set; }

    public string Description { set; get; }

    public string ModuleName { set; get; }
}


public class EndpointMappingsDto
{
    public PaginatedList<EndpointRequirementDto> AssignedRequirementsDto { get; set; }

    public PaginatedList<EndpointRequirementDto> AvailableRequirementsDto { get; set; }
}