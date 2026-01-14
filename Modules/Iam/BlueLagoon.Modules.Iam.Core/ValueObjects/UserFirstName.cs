using BlueLagoon.Modules.Iam.Core.Exceptions;

namespace BlueLagoon.Modules.Iam.Core.ValueObjects;

public sealed record UserFirstName
{
    public const int MinCharactersNumber = 3;
    public const int MaxCharactersNumber = 60;

    private UserFirstName() { }

    public string Value { get; }

    public UserFirstName(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length is < MinCharactersNumber or > MaxCharactersNumber)
            throw new InvalidUserFirstNameException(MinCharactersNumber, MaxCharactersNumber);

        Value = value;
    }

    public static implicit operator UserFirstName(string userFirstName) => new(userFirstName);

    public static implicit operator string(UserFirstName userFirstname) => userFirstname.Value;

    public override string ToString() => Value;

    public bool Contains(string value) => Value.Contains(value);
}