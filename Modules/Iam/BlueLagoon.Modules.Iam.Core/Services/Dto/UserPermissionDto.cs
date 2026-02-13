namespace BlueLagoon.Modules.Iam.Core.Services.Dto;

public class UserPermissionDto
{
    public int Id { get; set; }

    public Guid? RoleId { get; set; }

    public bool IsFromRole { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public string ModuleName { get; set; }

    public string RoleName { get; set; }
}
