using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal class ModuleNotFoundException(Guid Id) : BaseIamCoreException($"Moduł o id {Id} nie został odnalezionyy.", "011")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}