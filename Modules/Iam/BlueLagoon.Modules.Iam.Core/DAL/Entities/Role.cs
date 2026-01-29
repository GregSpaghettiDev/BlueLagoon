using BlueLagoon.Shared.DevTools.Base.Abstractions;
using BlueLagoon.Shared.DevTools.Base.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueLagoon.Modules.Iam.Core.DAL.Entities;

internal class Role : IdentityRole<Guid>, IBaseEntity
{
    protected Role() { }

    protected Role(Guid id, string roleName)
    {
        Id = id;
        Name = roleName;
        NormalizedName = roleName.ToUpperInvariant();
        Activate();
    }

    protected Role(ValueObjects.Role role)
    {
        Id = role.Id;
        Name = role.Value;
        NormalizedName = role.Value.ToUpperInvariant();
        Activate();
    }

    public DateTime CreatedAt { get; private set; }

    public Guid CreatorId { get; private set; }

    public DateTime? ModifiedAt { get; private set; }

    public Guid? ModificatorId { get; private set; }

    public bool IsActive { get; private set; }

    public static Role Create(ValueObjects.Role role)
        => new(role);

    public static Role Create(Guid id, ValueObjects.Role role)
        => new(id, role);

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
            throw new InvalidActivationFlagException(nameof(Role));

        if (!IsActive)
            IsActive = true;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidDeactivationFlagException(nameof(Role));

        if (IsActive)
            IsActive = false;
    }

    [InverseProperty(nameof(User.RoleCreators))]
    public virtual User Creator { get; private set; }

    [InverseProperty(nameof(User.RoleModificators))]
    public virtual User Modificator { get; private set; }
}