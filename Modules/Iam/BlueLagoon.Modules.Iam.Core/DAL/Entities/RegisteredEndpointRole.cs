using BlueLagoon.Shared.DevTools.Base.Abstractions;
using BlueLagoon.Shared.DevTools.Base.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueLagoon.Modules.Iam.Core.DAL.Entities;

//internal class RegisteredEndpointRole : IBaseEntity
//{

//    protected RegisteredEndpointRole()
//    {
//        Activate();
//    }

//    private RegisteredEndpointRole(Guid roleId, Guid regiteredEndpointId)
//    {
//        Activate();
//        RoleId = roleId;
//        RegisteredEndpointId = regiteredEndpointId;
//    }

//    public DateTime CreatedAt { get; private set; }

//    public Guid CreatorId { get; private set; }

//    public DateTime? ModifiedAt { get; private set; }

//    public Guid? ModificatorId { get; private set; }

//    public bool IsActive { get; private set; }

//    public Guid RegisteredEndpointId { get; private set; }

//    public Guid RoleId { get; private set; }

//    public static RegisteredEndpointRole Create(Guid roleId, Guid registeredEndpointId)
//        => new(roleId, registeredEndpointId);

//    public void SetCreatorAuditProperties(DateTime createdAt, Guid creatorId)
//    {
//        CreatedAt = createdAt;
//        CreatorId = creatorId;
//    }

//    public void SetModificatorAuditProperties(DateTime modifiedAt, Guid modificatorId)
//    {
//        ModifiedAt = modifiedAt;
//        ModificatorId = modificatorId;
//    }

//    public void Activate()
//    {
//        if (IsActive)
//            throw new InvalidActivationFlagException(nameof(Application));

//        if (!IsActive)
//            IsActive = true;
//    }

//    public void Deactivate()
//    {
//        if (!IsActive)
//            throw new InvalidDeactivationFlagException(nameof(Application));

//        if (IsActive)
//            IsActive = false;
//    }

//    public virtual RegisteredEndpoint RegisteredEndpoint { get; private set; }

//    public virtual Role Role { get; private set; }

//    [InverseProperty(nameof(User.RegisteredEndpointRoleCreators))]
//    public virtual User Creator { get; private set; }

//    [InverseProperty(nameof(User.RegisteredEndpointRoleModificators))]
//    public virtual User Modificator { get; private set; }
//}