namespace BlueLagoon.Modules.Iam.Api.Controllers.Permission.Requests;

public sealed class UpdatePermissionRequest
{
    public bool? IsActive {  get; set; }

    public string Description { get; set; }
}