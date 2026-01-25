using BlueLagoon.Modules.Iam.Core.Exceptions;

namespace BlueLagoon.Modules.Iam.Core.ValueObjects;

internal sealed record Role
{
    public const int MaxCharactersNumber = 256;
    public const int MinCharactersNumber = 3;

    public string Value { get; }

    public Guid Id { get; }

    public Role(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length < MinCharactersNumber || value.Length > MaxCharactersNumber)
            throw new InvalidRoleException(MinCharactersNumber, MaxCharactersNumber);

        Value = value;
        Id = Guid.NewGuid();
    }

    public Role(Guid id, string value)
    {
        Id = id;
        Value = value;
    }

    public static Role IamAdmin => new(new("81B75834-5CDA-4C7D-BC44-2CD49A118C06"), "iam-admin");

    public static implicit operator Role(string name) => new(name);
    public static implicit operator string(Role name) => name.Value;

    public override string ToString() => Value;
}