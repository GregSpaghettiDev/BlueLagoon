using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidOpenApiUrlException() : BaseIamCoreException("Należy podać url dla dokumentacji OpenApi", "002")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}