using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal sealed class InvalidEmailException(string code, string description) 
    : BaseIamCoreException($"Email nie spełnia reguł walidacji.", "017", new Exception($"IdentityErrorCode: {code} Description: {description}"))
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
}