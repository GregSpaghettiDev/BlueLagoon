using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidOperationSummaryException(int MinCharactersNumber, int MaxCharactersNumber) 
    : BaseIamCoreException($"Podsumowanie może zawierać od {MinCharactersNumber} do {MaxCharactersNumber} znaków.", "005")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}