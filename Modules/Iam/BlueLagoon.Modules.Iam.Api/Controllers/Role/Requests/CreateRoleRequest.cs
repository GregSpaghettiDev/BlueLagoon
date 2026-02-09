namespace BlueLagoon.Modules.Iam.Api.Controllers.Role.Requests;

internal class CreateRoleRequest
{
    public string Name { get; set; }

    public string Code { get; set; }

    public IList<Guid> RoleIds { get; set; }
}
