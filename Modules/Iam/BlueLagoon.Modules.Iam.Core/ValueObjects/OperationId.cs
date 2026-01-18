using BlueLagoon.Modules.Iam.Core.Exceptions;

namespace BlueLagoon.Modules.Iam.Core.ValueObjects;

internal sealed record OperationId(string Value)
{
    public const int MinCharactersNumber = 3;
    public const int MaxCharactersNumber = 50;

    public string Value { get; } = (string.IsNullOrWhiteSpace(Value) || Value.Length is < MinCharactersNumber or > MaxCharactersNumber)
                                        ? throw new InvalidOperationIdException(MinCharactersNumber, MaxCharactersNumber)
                                        : Value;

    public static implicit operator OperationId(string name) => new(name);

    public static implicit operator string(OperationId name) => name.Value;

    public override string ToString() => Value;

    public bool Contains(string value) => Value.Contains(value);
}