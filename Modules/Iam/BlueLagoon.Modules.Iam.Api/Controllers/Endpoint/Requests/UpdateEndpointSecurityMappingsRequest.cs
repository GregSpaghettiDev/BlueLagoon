namespace BlueLagoon.Modules.Iam.Api.Controllers.Endpoint.Requests;

public sealed class UpdateEndpointSecurityMappingsRequest
{
    public IList<Guid> RequirementIds { get; set; }
}