using BlueLagoon.Shared.DevTools.Base.Abstractions;
using BlueLagoon.Shared.DevTools.Base.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueLagoon.Modules.Iam.Core.DAL.Entities;

internal class UserRole : IdentityUserRole<Guid>, IBaseEntity
{
    public UserRole()
    {
        Activate();
    }

    public DateTime CreatedAt { get; private set; }

    public Guid CreatorId { get; private set; }

    public DateTime? ModifiedAt { get; private set; }

    public Guid? ModificatorId { get; private set; }

    public bool IsActive { get; private set; }

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
            throw new InvalidActivationFlagException(nameof(UserRole));

        if (!IsActive)
            IsActive = true;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidDeactivationFlagException(nameof(UserRole));

        if (IsActive)
            IsActive = false;
    }

    [InverseProperty(nameof(User.UserRoleCreators))]
    public virtual User Creator { get; private set; }

    [InverseProperty(nameof(User.UserRoleModificators))]
    public virtual User Modificator { get; private set; }

    public virtual User User { get; private set; }

    public virtual Role Role { get; private set; }
}