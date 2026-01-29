namespace BlueLagoon.Shared.DevTools.Base.Abstractions;

public interface IBaseEntity
{
    DateTime CreatedAt { get; }

    Guid CreatorId { get;  }

    DateTime? ModifiedAt { get; }

    Guid? ModificatorId { get; }

    bool IsActive { get; }

    void SetCreatorAuditProperties(DateTime createdAt, Guid creatorId);

    void SetModificatorAuditProperties(DateTime modifiedAt, Guid modificatorId);

    void Activate();

    void Deactivate();
}
