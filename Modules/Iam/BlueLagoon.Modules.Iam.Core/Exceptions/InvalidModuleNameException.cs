using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidModuleNameException(int MinCharactersNumber, int MaxCharactersNumber) : BaseCoreException
{
    public override string Message { get; } = $"Nazwa modułu może zawierać od {MinCharactersNumber} do {MaxCharactersNumber} znaków.";
}