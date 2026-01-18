using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidOperationIdException(int MinCharactersNumber, int MaxCharactersNumber) : BaseCoreException
{
    public override string Message { get; } = $"Id operacji może zawierać od {MinCharactersNumber} do {MaxCharactersNumber} znaków.";
}