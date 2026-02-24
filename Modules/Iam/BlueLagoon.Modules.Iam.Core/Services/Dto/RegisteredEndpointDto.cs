namespace BlueLagoon.Modules.Iam.Core.Services.Dto;

public class RegisteredEndpointDto
{
    public Guid Id { get; set; }

    public string ModuleName { get; set; }

    public string Path { get; set; }

    public string OperationId { get; set; }

    public Guid CreatorId { get; set; }

    public string CreatorName { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? ModificatorId { get; set; }

    public string ModificatorName { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public bool HasPermissionMappings { get; set; }
}