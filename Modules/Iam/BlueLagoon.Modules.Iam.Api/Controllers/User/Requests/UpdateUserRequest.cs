namespace BlueLagoon.Modules.Iam.Api.Controllers.User.Requests;

public sealed class UpdateUserRequest
{
    public bool? IsActive { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public IList<Guid> RoleIds { get; set; }

    public IList<Guid> PermissionIds { get; set; }
}

public sealed class  RoleRequest
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}
