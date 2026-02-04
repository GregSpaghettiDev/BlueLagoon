using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidOperationDescriptionException(int MinCharactersNumber, int MaxCharactersNumber) 
    : BaseIamCoreException($"Opis endpointu może zawierać od {MinCharactersNumber} do {MaxCharactersNumber} znaków.", "003")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}

