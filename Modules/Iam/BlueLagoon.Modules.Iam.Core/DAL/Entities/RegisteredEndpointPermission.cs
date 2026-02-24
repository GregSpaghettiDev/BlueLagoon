using BlueLagoon.Shared.DevTools.Base.Abstractions;
using BlueLagoon.Shared.DevTools.Base.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueLagoon.Modules.Iam.Core.DAL.Entities;

internal class RegisteredEndpointPermission : IBaseEntity
{

    protected RegisteredEndpointPermission() 
    {
        Activate();
    }

    private RegisteredEndpointPermission(Guid permissionId, Guid regiteredEndpointId)
    {
        Activate();
        PermissionId = permissionId;
        RegisteredEndpointId = regiteredEndpointId;
    }

    public DateTime CreatedAt { get; private set; }

    public Guid CreatorId { get; private set; }

    public DateTime? ModifiedAt { get; private set; }

    public Guid? ModificatorId { get; private set; }

    public bool IsActive { get; private set; }

    public Guid RegisteredEndpointId { get; private set; }

    public Guid PermissionId { get; private set; }

    public static RegisteredEndpointPermission Create(Guid permissionId, Guid registeredEndpointId)
        => new(permissionId, registeredEndpointId);

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
            throw new InvalidActivationFlagException(nameof(Application));

        if (!IsActive)
            IsActive = true;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidDeactivationFlagException(nameof(Application));

        if (IsActive)
            IsActive = false;
    }

    public virtual RegisteredEndpoint RegisteredEndpoint { get; private set; }

    public virtual Permission Permission { get; private set; }

    [InverseProperty(nameof(User.RegisteredEndpointPermissionCreators))]
    public virtual User Creator { get; private set; }

    [InverseProperty(nameof(User.RegisteredEndpointPermissionModificators))]
    public virtual User Modificator { get; private set; }
}