using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

public sealed class InvalidPathException() : BaseCoreException
{
    public override string Message { get; } = $"Niepoprawna ścieżka endpointa";
}
