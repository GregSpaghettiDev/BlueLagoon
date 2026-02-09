using BlueLagoon.Modules.Iam.Core.Exceptions;

namespace BlueLagoon.Modules.Iam.Core.ValueObjects;

internal sealed record PermissionName(string Name, string ModuleName)
{
    public const int MinCharactersNumber = 3;
    public const int MaxCharactersNumber = 256;

    public string FullPermissionName => string.IsNullOrWhiteSpace(Name) || Name.Length is < MinCharactersNumber or > MaxCharactersNumber || string.IsNullOrWhiteSpace(ModuleName)
                                                                         ? throw new InvalidPermissionNameException(MinCharactersNumber, MaxCharactersNumber)
                                                                         : $"{ModuleName}.{Name}";
    public override string ToString() => FullPermissionName;
}
