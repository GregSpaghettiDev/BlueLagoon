namespace BlueLagoon.Modules.Iam.Core.Services.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatorName { get; set; }

    public string AssignedModules { get; set; }

    public UserOptionsDto Options { get; set; }
}
