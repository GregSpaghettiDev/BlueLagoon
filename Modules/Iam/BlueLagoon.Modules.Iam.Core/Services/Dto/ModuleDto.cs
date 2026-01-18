namespace BlueLagoon.Modules.Iam.Core.Services.Dto;

public class ModuleDto
{
    public Guid Id { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatorId { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public Guid? ModificatorId { get; set; }

    public bool IsActive { get; set; }

    public ModuleDtoOptionsDto Options { get; set; }
}

public class ModuleDtoOptionsDto
{
    public bool IsDeactivate { get; set; }
}
