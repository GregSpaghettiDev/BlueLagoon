using BlueLagoon.Shared.DevTools.Base.Exceptions;

namespace BlueLagoon.Shared.DevTools.Base.Abstractions;

public abstract class BaseEntity<TEntity> : IBaseEntity
    where TEntity : class, IBaseEntity
{
    protected BaseEntity()
    {
    }

    public virtual Guid Id { get; protected set; }

    public virtual DateTime CreatedAt { get; protected set; }

    public virtual Guid CreatorId { get; protected set; }

    public virtual DateTime? ModifiedAt { get; protected set; }

    public virtual Guid? ModificatorId { get; protected set; }

    public virtual bool IsActive { get; protected set; }

    public void SetCreatorAuditProperties(DateTime createdAt, Guid creatorId)
    {
        CreatedAt = createdAt;
        CreatorId = creatorId;
    }

    public void SetModificatorAuditProperties(DateTime modifiedAt, Guid modificatorId)
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