namespace BlueLagoon.Modules.Iam.Api.Controllers.Role.Requests;

internal class UpdateRolePermissionsRequest
{
    public IList<Guid> PermissionIdsToReplace { get; set; }
}
