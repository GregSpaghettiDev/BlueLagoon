namespace BlueLagoon.Modules.Iam.Api.Controllers.Permission.Requests;

public sealed class CreatePermissionRequest
{
    public string Name { get; set; }

    public string Description { get; set; }

    public Guid ModuleId {  get; set; }
}