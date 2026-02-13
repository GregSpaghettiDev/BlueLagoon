using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using BlueLagoon.Shared.DevTools.Tools;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidUserFirstNameException(int minCharactersNumber, int maxCharactersNumber) 
    : BaseIamCoreException($"Imię użytkownika musi zawierać minimum {NounsGradator.GradateTheNounAccordingToQuantity(minCharactersNumber, "znak", "znaki", "znaków")} oraz maksymalnie {NounsGradator.GradateTheNounAccordingToQuantity(maxCharactersNumber, "znak", "znaki", "znaków")}.", "009")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}