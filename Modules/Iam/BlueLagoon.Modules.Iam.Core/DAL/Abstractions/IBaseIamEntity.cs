using BlueLagoon.Shared.DevTools.Base;
using BlueLagoon.Shared.DevTools.Base.Abstractions;

namespace BlueLagoon.Modules.Iam.Core.DAL.Abstractions;

internal interface IBaseIamEntity : IBaseEntity
{
    public BaseEntity MetaData { get; }
}
