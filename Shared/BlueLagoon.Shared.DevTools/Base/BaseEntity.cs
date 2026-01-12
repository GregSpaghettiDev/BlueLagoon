namespace BlueLagoon.Shared.DevTools.Base;

public class BaseEntity
{
    public BaseEntity()
    {
    }

    public BaseId Id { get; protected set; }

    public BaseDate CreatedAt { get; protected set; }

    public BaseId CreatorId { get; protected set; }

    public BaseDate ModifiedAt { get; protected set; }

    public BaseId ModificatorId { get; protected set; }

    public bool IsActive { get; protected set; }

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