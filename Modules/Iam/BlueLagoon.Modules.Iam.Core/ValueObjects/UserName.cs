using BlueLagoon.Shared.DevTools.Tools;

namespace BlueLagoon.Modules.Iam.Core.ValueObjects;

public sealed record UserName(UserFirstName FirstName, UserLastName LastName)
{
    public string Value { get; } = string.Concat(FirstName, ".", LastName).RemovePolishDiacritics();

    public static implicit operator string(UserName userLastName) => userLastName.Value;

    public override string ToString() => Value;
}