using BlueLagoon.Shared.DevTools.Uniqueidentifier;

namespace BlueLagoon.Shared.DevTools.Base.Abstractions;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
    }

    public virtual BaseId Id { get; protected set; }

    public virtual BaseDate CreatedAt { get; protected set; }

    public virtual BaseId CreatorId { get; protected set; }

    public virtual BaseDate ModifiedAt { get; protected set; }

    public virtual BaseId ModificatorId { get; protected set; }

    public virtual bool IsActive { get; protected set; }

    public void SetCreatorAuditProperties(BaseDate createdAt, BaseId creatorId)
    {
        CreatedAt = createdAt;
        CreatorId = creatorId;
    }

    public void SetModificatorAuditProperties(BaseDate modifiedAt, BaseId modificatorId)
    {
        ModifiedAt = modifiedAt;
        ModificatorId = modificatorId;
    }
}