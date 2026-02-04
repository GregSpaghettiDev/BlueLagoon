using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using BlueLagoon.Shared.DevTools.Tools;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidScopeException(int MinCharactersNumber) 
    : BaseIamCoreException($"Niepoprawna nazwa aplikacji. Nazwa aplikacji musi zawierać minimum {NounsGradator.GradateTheNounAccordingToQuantity(MinCharactersNumber, "znak", "znaki", "znaków")}.", "007")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
