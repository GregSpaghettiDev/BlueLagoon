using BlueLagoon.Modules.Iam.Core.Exceptions;

namespace BlueLagoon.Modules.Iam.Core.ValueObjects;

internal sealed record OperationDescription(string Value)
{
    public const int MinCharactersNumber = 3;
    public const int MaxCharactersNumber = 250;

    public string Value { get; } = (string.IsNullOrWhiteSpace(Value) || Value.Length is < MinCharactersNumber or > MaxCharactersNumber)
                                        ? throw new InvalidOperationDescriptionException(MinCharactersNumber, MaxCharactersNumber)
                                        : Value;

    public static implicit operator OperationDescription(string name) => new(name);

    public static implicit operator string(OperationDescription name) => name.Value;

    public override string ToString() => Value;

    public bool Contains(string value) => Value.Contains(value);
}