using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using System.Net;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidPathException() : BaseIamCoreException("Niepoprawna ścieżka endpointa", "006")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
}
