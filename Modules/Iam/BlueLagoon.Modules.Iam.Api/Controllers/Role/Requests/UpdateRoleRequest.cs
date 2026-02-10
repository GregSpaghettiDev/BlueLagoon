namespace BlueLagoon.Modules.Iam.Api.Controllers.Role.Requests;

internal class UpdateRoleRequest
{
    public bool? IsActive { get; set; }

    public IList<Guid> PermissionIds { get; set; }
}
