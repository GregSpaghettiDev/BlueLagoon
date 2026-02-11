using BlueLagoon.Shared.Infrastructure.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal sealed class RoleAlreadyExistsException(string id, string name, string displayName) 
    : BaseCoreException($"Rola dla podanych danych \"{id}\", \"{name}\", \"{displayName}\" istnieje już w systemie", "013")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
}
