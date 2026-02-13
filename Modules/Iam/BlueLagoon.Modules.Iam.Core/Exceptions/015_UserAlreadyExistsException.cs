using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal sealed class UserAlreadyExistsException(string code, string description) 
    : BaseIamCoreException("Użytkownik o został już dodany do systemu.", "015", new Exception($"IdentityErrorCode: {code} Description: {description}"))
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
}