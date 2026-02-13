using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidOperationIdException(int minCharactersNumber, int maxCharactersNumber) 
    : BaseIamCoreException($"Id operacji może zawierać od {minCharactersNumber} do {maxCharactersNumber} znaków.", "004")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}