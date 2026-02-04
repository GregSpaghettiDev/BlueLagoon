using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using BlueLagoon.Shared.DevTools.Tools;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidUserFirstNameException(int MinCharactersNumber, int MaxCharactersNumber) 
    : BaseIamCoreException($"Imię użytkownika musi zawierać minimum {NounsGradator.GradateTheNounAccordingToQuantity(MinCharactersNumber, "znak", "znaki", "znaków")} oraz maksymalnie {NounsGradator.GradateTheNounAccordingToQuantity(MaxCharactersNumber, "znak", "znaki", "znaków")}.", "008")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}