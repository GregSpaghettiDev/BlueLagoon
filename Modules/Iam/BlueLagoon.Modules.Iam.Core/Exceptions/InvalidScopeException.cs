using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using BlueLagoon.Shared.DevTools.Tools;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidScopeException(int MinCharactersNumber) : BaseCoreException
{
    public override string Message { get; } = $"Niepoprawna nazwa aplikacji. Nazwa aplikacji musi zawierać minimum {NounsGradator.GradateTheNounAccordingToQuantity(MinCharactersNumber, "znak", "znaki", "znaków")}.";
}
