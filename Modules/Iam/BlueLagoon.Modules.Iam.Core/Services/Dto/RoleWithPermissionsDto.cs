namespace BlueLagoon.Modules.Iam.Core.Services.Dto;

public class RoleWithPermissionsDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; }

    public bool IsActive { get; set; }

    public IEnumerable<ClaimDto> AssignedPermissions { get; set; }

    public IEnumerable<ClaimDto> AvailablePermissions { get; set; }

    public RoleOptionsDto Options { get; set; }
}

public class ClaimDto
{
    public Guid Id { get; set; }

    public Guid? ClaimId { get; set; }

    public string Code { set; get; }

    public string Type { get; set; }

    public string Description { set; get; }

    public string ModuleName { set; get; }
}