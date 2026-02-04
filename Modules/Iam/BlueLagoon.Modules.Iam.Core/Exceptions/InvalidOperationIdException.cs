using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidOperationIdException(int MinCharactersNumber, int MaxCharactersNumber) 
    : BaseIamCoreException($"Id operacji może zawierać od {MinCharactersNumber} do {MaxCharactersNumber} znaków.", "004")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}