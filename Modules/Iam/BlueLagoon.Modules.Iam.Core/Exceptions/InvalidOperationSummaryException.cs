using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidOperationSummaryException(int MinCharactersNumber, int MaxCharactersNumber) : BaseCoreException
{
    public override string Message { get; } = $"Podsumowanie może zawierać od {MinCharactersNumber} do {MaxCharactersNumber} znaków.";
}