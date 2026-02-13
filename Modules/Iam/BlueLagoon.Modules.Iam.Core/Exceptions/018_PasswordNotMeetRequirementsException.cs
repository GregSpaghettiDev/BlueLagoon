using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal sealed class PasswordNotMeetRequirementsException(string code, string description)
    : BaseIamCoreException("Hasło nie spełnia wymagań systemowych.", "018", new Exception($"IdentityErrorCode: {code} Description: {description}"))
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
}