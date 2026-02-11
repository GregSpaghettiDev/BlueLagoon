using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidOperationDescriptionException(int minCharactersNumber, int maxCharactersNumber) 
    : BaseIamCoreException($"Opis endpointu może zawierać od {minCharactersNumber} do {maxCharactersNumber} znaków.", "003")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}

