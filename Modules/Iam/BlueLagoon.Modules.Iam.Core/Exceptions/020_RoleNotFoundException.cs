using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal class RoleNotFoundException(Guid id) : BaseIamCoreException($"Rola o id {id} nie została odnaleziona.", "020")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}