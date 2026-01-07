using Common.BaseValueObjects;
using Common.UniqueIdentifier;

namespace Common.EntityFramework.Base
{
    public class BaseEntity
    {
        protected BaseEntity()
        {
        }

        public virtual BaseId Id { get; protected set; }

        public virtual BaseDate CreatedAt { get; protected set; }

        public virtual BaseId CreatorId { get; protected set; }

        public virtual BaseId CreatorContextId { get; protected set; }

        public virtual BaseDate ModifiedAt { get; protected set; }

        public virtual BaseId ModificatorId { get; protected set; }

        public virtual BaseId ModificatorContextId { get; protected set; }

        public virtual bool IsActive { get; protected set; }

        public byte[] RowVersion { get; protected set; } = default!;

        public void SetCreatorAuditProperties(BaseDate createdAt, BaseId creatorId, BaseId creatorContextId = null)
        {
            CreatedAt = createdAt;
            CreatorId = creatorId;
            CreatorContextId = creatorContextId?.Value.IsNullOrEmpty() ?? true ? null : creatorContextId;
        }

        public void SetModificatorAuditProperties(BaseDate modifiedAt, BaseId modificatorId, BaseId modificatorContextId = null)
        {
            ModifiedAt = modifiedAt;
            ModificatorId = modificatorId;
            ModificatorContextId = modificatorContextId?.Value.IsNullOrEmpty() ?? true ? null : modificatorContextId;
        }
    }
}