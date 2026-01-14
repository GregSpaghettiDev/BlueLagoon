using BlueLagoon.Modules.Iam.Core.Exceptions;

namespace BlueLagoon.Modules.Iam.Core.ValueObjects;

public sealed record UserLastName(string Value)
{
    public const int MinCharactersNumber = 3;
    public const int MaxCharactersNumber = 100;

    public string Value { get; } = string.IsNullOrWhiteSpace(Value) || Value.Length is < MinCharactersNumber or > MaxCharactersNumber 
                                            ? throw new InvalidUserLastNameException(MinCharactersNumber, MaxCharactersNumber) 
                                            : Value;

    public static implicit operator UserLastName(string userLastName) => new(userLastName);

    public static implicit operator string(UserLastName userLastName) => userLastName.Value;

    public override string ToString() => Value;

    public bool Contains(string value) => Value.Contains(value);
}