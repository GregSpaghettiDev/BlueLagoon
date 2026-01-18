using BlueLagoon.Modules.Iam.Core.Exceptions;
using System.Text.RegularExpressions;

namespace BlueLagoon.Modules.Iam.Core.ValueObjects;

internal sealed record Path(string Value)
{
    private static readonly Regex regex = new(@"^\/([a-zA-Z0-9_-]+|{[a-zA-Z0-9_-]+})(\/([a-zA-Z0-9_-]+|{[a-zA-Z0-9_-]+}))*\/?$", RegexOptions.Compiled);

    public string Value { get; } = string.IsNullOrWhiteSpace(Value) || !regex.IsMatch(Value)
                                        ? throw new InvalidPathException()
                                        : Value;

    public static implicit operator Path(string name) => new(name);

    public static implicit operator string(Path name) => name.Value;

    public override string ToString() => Value;

    public bool Contains(string value) => Value.Contains(value);
}