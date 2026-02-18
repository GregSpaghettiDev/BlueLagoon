namespace BlueLagoon.Modules.Iam.Api.Controllers.User.Requests;

public sealed class UpdateUserRequest
{
    public bool? IsActive { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public IList<string> RoleNames { get; set; }

    public IList<string> PermissionNames { get; set; }
}