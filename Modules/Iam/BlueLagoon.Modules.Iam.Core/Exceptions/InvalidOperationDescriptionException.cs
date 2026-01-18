using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidOperationDescriptionException(int MinCharactersNumber, int MaxCharactersNumber) : BaseCoreException
{
    public override string Message { get; } = $"Opis endpointu może zawierać od {MinCharactersNumber} do {MaxCharactersNumber} znaków.";
}