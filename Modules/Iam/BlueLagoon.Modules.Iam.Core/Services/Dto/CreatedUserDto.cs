namespace BlueLagoon.Modules.Iam.Core.Services.Dto;

public class CreatedUserDto
{
    public Guid Id { get; set; }

    public string UserName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public IEnumerable<PermissionOccuredInRoleDto> PermissionsOccuredInRoles { get; set; }

}

public class PermissionOccuredInRoleDto
{
    public Guid Id { get; set;  }

    public string Name { get; set; }

    public string ModuleName { get; set; }

    public string Description { get; set; }
}