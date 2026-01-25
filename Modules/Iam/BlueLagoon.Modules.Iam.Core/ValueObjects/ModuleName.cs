using BlueLagoon.Modules.Iam.Core.Exceptions;

namespace BlueLagoon.Modules.Iam.Core.ValueObjects;

internal sealed record ModuleName(string Value)
{
    public const int MinCharactersNumber = 3;
    public const int MaxCharactersNumber = 50;

    public string Value { get; } = (string.IsNullOrWhiteSpace(Value) || Value.Length is < MinCharactersNumber or > MaxCharactersNumber)
                                        ? throw new InvalidModuleNameException(MinCharactersNumber, MaxCharactersNumber)
                                        : Value.ToLowerInvariant();

    public static implicit operator ModuleName(string name) => new(name);

    public static implicit operator string(ModuleName name) => name.Value;

    public override string ToString() => Value;

    public bool Contains(string value) => Value.Contains(value);
}
