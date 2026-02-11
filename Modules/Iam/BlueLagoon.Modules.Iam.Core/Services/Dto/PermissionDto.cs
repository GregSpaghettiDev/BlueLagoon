namespace BlueLagoon.Modules.Iam.Core.Services.Dto;

public class PermissionDto
{
    public Guid Id { get; set; }

    public string Code { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatorId { get; set;  }

    public string CreatorName { get; set; }

    public bool IsActive { get; set; }

    public string Description { get; set; }

    public PermissionOptionsDto Options { get; set; }
}

public class PermissionOptionsDto
{
    public bool IsDetails { get; set; } = true;

    public bool IsActivate { get; set; }

    public bool IsDelete { get; set; }
}
