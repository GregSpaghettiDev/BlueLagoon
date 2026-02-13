using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal sealed class InvalidUserNameException(string code, string description) 
    : BaseIamCoreException($"Podana nazwa użytkownika zawiera niedozwolone znaki lub jest pusta.", "016", new Exception($"IdentityErrorCode: {code} Description: {description}"))
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
}