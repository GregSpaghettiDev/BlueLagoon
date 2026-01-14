using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using BlueLagoon.Shared.DevTools.Tools;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidUserFirstNameException(int MinCharactersNumber, int MaxCharactersNumber) : BaseCoreException
{
    public override string Message { get; } 
        = $"Imię użytkownika musi zawierać minimum {NounsGradator.GradateTheNounAccordingToQuantity(MinCharactersNumber, "znak", "znaki", "znaków")} oraz maksymalnie {NounsGradator.GradateTheNounAccordingToQuantity(MaxCharactersNumber, "znak", "znaki", "znaków")}.";
}