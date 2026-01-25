using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;

namespace BlueLagoon.Modules.Iam.Core.Exceptions;

internal class ModuleNotFoundException(Guid Id) : BaseCoreException
{
    public override string Message { get; } = $"Moduł o id {Id} nie został odnalezionyy.";
}