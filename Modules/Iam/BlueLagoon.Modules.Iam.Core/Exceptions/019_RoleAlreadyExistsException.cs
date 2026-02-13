using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal sealed class RoleAlreadyExistsException(string id, string name, string displayName) 
    : BaseIamCoreException($"Rola dla podanych danych \"{id}\", \"{name}\", \"{displayName}\" istnieje już w systemie", "019")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
}
