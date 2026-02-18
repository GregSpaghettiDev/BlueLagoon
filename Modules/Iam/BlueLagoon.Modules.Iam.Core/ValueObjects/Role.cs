using BlueLagoon.Modules.Iam.Core.Exceptions;

namespace BlueLagoon.Modules.Iam.Core.ValueObjects;

public sealed record Role
{
    public const int MaxCharactersNumber = 256;
    public const int MinCharactersNumber = 3;

    public string Name { get; }

    public string NormalizedName => Name.ToUpperInvariant();

    public string DisplayRoleName { get; }

    public Guid Id { get; }

    public Role(Guid id, string name, string displayRoleName)
    {
        if (string.IsNullOrWhiteSpace(name)
            || name.Length < MinCharactersNumber
            || name.Length > MaxCharactersNumber
            || string.IsNullOrWhiteSpace(displayRoleName)
            || displayRoleName.Length < MinCharactersNumber
            || displayRoleName.Length > MaxCharactersNumber)
            throw new InvalidRoleException(MinCharactersNumber, MaxCharactersNumber);

        Id = id;
        Name = name;
        DisplayRoleName = displayRoleName;
    }

    public static Role IamAdmin => new(new("81B75834-5CDA-4C7D-BC44-2CD49A118C06"), "iam-admin", "Administrator modułu IAM");

    public const string IamAdminRoleName = "iam-admin";

    public const string IamAdminDisplayRoleName = "Administrator modułu IAM";
}