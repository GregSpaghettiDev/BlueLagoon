using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal class PermissionNotFoundException(Guid id) : BaseIamCoreException($"Uprawnienie o id {id} nie zostało odnalezione.", "014")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}