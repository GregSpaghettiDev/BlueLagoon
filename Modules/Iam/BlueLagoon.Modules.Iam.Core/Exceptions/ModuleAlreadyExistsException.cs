using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal class ModuleAlreadyExistsException(string Name) : BaseCoreException
{
    public override string Message { get; } = $"Moduł o nazwie {Name} istnieje już w systemie.";
}