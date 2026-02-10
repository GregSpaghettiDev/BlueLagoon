namespace BlueLagoon.Modules.Iam.Api.Controllers.Module.Requests;

public sealed class UpdateModuleRequest
{
    public string Name { get; set; }

    public string OpenApiPath { get; set; }

    public string BaseUrl { get; set; }

    public bool? IsActive { get; set; }
}
