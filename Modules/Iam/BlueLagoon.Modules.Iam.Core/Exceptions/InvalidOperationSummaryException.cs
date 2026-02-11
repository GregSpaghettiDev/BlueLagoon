using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidOperationSummaryException(int minCharactersNumber, int maxCharactersNumber) 
    : BaseIamCoreException($"Podsumowanie może zawierać od {minCharactersNumber} do {maxCharactersNumber} znaków.", "005")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}