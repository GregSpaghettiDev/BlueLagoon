using Common.BaseValueObjects;

namespace Common.EntityFramework.Base
{
    public interface IBaseEntity
    {
        BaseId Id { get; }

        BaseDate CreatedAt { get; }

        BaseId CreatorId { get; }

        BaseDate ModifiedAt { get; }

        BaseId ModificatorId { get; }

        bool IsActive { get; }
    }
}
