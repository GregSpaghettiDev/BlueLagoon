namespace BlueLagoon.Modules.Iam.Core.Services.Dto;

public class UserRoleDto
{
    public Guid RoleId { get; set; }

    public string RoleCode { get; set; }

    public string RoleName { get; set; }

    public bool IsActive { get; set; }

    public string AccesingModules { get; set; }
}