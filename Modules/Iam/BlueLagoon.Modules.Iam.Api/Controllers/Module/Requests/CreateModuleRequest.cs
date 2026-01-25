namespace BlueLagoon.Modules.Iam.Api.Controllers.Module.Requests;

public sealed class CreateModuleRequest
{
    public string Name { get; init; }

    public string Description { get; init; }
    
    public string OpenApiUri { get; init; }
    
    public string BaseUrl { get; init; }
}