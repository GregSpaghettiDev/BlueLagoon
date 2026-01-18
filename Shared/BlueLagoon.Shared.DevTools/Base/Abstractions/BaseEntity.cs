using BlueLagoon.Shared.DevTools.Base.Exceptions;

namespace BlueLagoon.Shared.DevTools.Base.Abstractions;

public abstract class BaseEntity<TEntity> : IBaseEntity
    where TEntity : class, IBaseEntity
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

    public void Activate()
    {
        if (IsActive)
            throw new InvalidActivationFlagException(typeof(TEntity).Name);

        if (!IsActive)
            IsActive = true;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidActivationFlagException(typeof(TEntity).Name);

        if (IsActive)
            IsActive = false;
    }
}