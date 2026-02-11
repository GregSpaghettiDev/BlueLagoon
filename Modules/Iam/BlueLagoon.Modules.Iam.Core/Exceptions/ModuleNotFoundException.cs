using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal class ModuleNotFoundException(Guid id) : BaseIamCoreException($"Moduł o id {id} nie został odnaleziony.", "011")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}