using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal class RoleNotFoundException(Guid Id) : BaseIamCoreException($"Rola o id {Id} nie została odnalezionya.", "012")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}