using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal class ModuleAlreadyExistsException(string name) : BaseIamCoreException($"Moduł o nazwie {name} istnieje już w systemie.", "010")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
}