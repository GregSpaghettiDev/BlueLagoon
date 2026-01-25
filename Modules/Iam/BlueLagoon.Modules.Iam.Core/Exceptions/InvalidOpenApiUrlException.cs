using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidOpenApiUrlException() : BaseCoreException
{
    public override string Message { get; } = "Należy podać url dla dokumentacji OpenApi";
}