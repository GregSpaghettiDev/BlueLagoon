using BlueLagoon.Modules.Iam.Core.ValueObjects;
using BlueLagoon.Shared.DevTools.Base;
using BlueLagoon.Shared.DevTools.Base.Abstractions;
using BlueLagoon.Shared.DevTools.Base.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace BlueLagoon.Modules.Iam.Core.DAL.Entities;

internal class User : IdentityUser<BaseId>, IBaseEntity
{
    public User()
    {
        Activate();
    }

    public UserFirstName FirstName { get; set; }

    public UserLastName LastName { get; set; }

    public BaseDate CreatedAt { get; private set; }

    public BaseId CreatorId { get; private set; }

    public BaseDate ModifiedAt { get; private set; }

    public BaseId ModificatorId { get; private set; }

    public bool IsActive { get; private set; }

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
            throw new InvalidActivationFlagException(nameof(User));

        if (!IsActive)
            IsActive = true;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidDeactivationFlagException(nameof(User));

        if (IsActive)
            IsActive = false;
    }

    #region nav properties
    public virtual User Creator { get; private set; }

    public virtual User Modificator { get; private set; }

    public virtual ICollection<User> UserCreators { get; private set; }

    public virtual ICollection<User> UserModificators { get; private set; }

    public virtual ICollection<UserToken> UserTokenCreators { get; private set; }

    public virtual ICollection<UserToken> UserTokenModificators { get; private set; }

    public virtual ICollection<Application> ApplicationCreators { get; private set; }

    public virtual ICollection<Application> ApplicationModificators { get; private set; }

    public virtual ICollection<Authorization> AuthorizationCreators { get; private set; }

    public virtual ICollection<Authorization> AuthorizationModificators { get; private set; }

    public virtual ICollection<Module> ModuleCreators { get; private set; }

    public virtual ICollection<Module> ModuleModificators { get; private set; }

    public virtual ICollection<Role> RoleCreators { get; private set; }

    public virtual ICollection<Role> RoleModificators { get; private set; }

    public virtual ICollection<RoleClaim> RoleClaimCreators { get; private set; }

    public virtual ICollection<RoleClaim> RoleClaimModificators { get; private set; }

    public virtual ICollection<Token> TokenCreators { get; private set; }

    public virtual ICollection<Token> TokenModificators { get; private set; }

    public virtual ICollection<UserClaim> UserClaimCreators { get; private set; }

    public virtual ICollection<UserClaim> UserClaimModificators { get; private set; }

    public virtual ICollection<UserLogin> UserLoginCreators { get; private set; }

    public virtual ICollection<UserLogin> UserLoginModificators { get; private set; }

    public virtual ICollection<UserRole> UserRoleCreators { get; private set; }

    public virtual ICollection<UserRole> UserRoleModificators { get; private set; }

    public virtual ICollection<RegisteredEndpoint> RegisteredEndpointCreators { get; private set; }

    public virtual ICollection<RegisteredEndpoint> RegisteredEndpointModificators { get; private set; }
    #endregion

}