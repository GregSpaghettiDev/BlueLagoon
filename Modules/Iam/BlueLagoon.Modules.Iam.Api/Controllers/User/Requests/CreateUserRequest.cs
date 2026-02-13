namespace BlueLagoon.Modules.Iam.Api.Controllers.User.Requests;

public sealed class CreateUserRequest
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string RepeatedPassword { get; set; }

    public string PhoneNumber { get; set; }

    public IEnumerable<Guid> RoleIds { get; set; }

    public IEnumerable<Guid> PermissionIds { get; set; }
}
