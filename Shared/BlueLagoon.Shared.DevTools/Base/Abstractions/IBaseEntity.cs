namespace BlueLagoon.Shared.DevTools.Base.Abstractions;

public interface IBaseEntity
{
    BaseDate CreatedAt { get; }

    BaseId CreatorId { get;  }

    BaseDate ModifiedAt { get; }

    BaseId ModificatorId { get; }

    bool IsActive { get; }

    void SetCreatorAuditProperties(BaseDate createdAt, BaseId creatorId);

    void SetModificatorAuditProperties(BaseDate modifiedAt, BaseId modificatorId);

    void Activate();

    void Deactivate();
}
