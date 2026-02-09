namespace BlueLagoon.Modules.Iam.Core.Services.Dto;

public class RoleDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; }

    public bool IsActive { get; set; }

    public RoleOptionsDto Options { get; set; }
}

public class RoleOptionsDto
{
    public bool IsDetails => true;

    public bool IsDeactivate { get; set; }

    public bool IsDelete { get; set; }
}
