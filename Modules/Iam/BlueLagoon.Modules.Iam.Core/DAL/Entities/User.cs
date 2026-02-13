using BlueLagoon.Modules.Iam.Core.ValueObjects;
using BlueLagoon.Shared.DevTools.Base.Abstractions;
using BlueLagoon.Shared.DevTools.Base.Exceptions;
using BlueLagoon.Shared.DevTools.Tools;
using Microsoft.AspNetCore.Identity;

namespace BlueLagoon.Modules.Iam.Core.DAL.Entities;

internal class User : IdentityUser<Guid>, IBaseEntity
{
    public User()
    {
        Activate();

        #region nav props initialization
        UserRoles = new HashSet<UserRole>();
        UserLogins = new HashSet<UserLogin>();
        UserTokens = new HashSet<UserToken>();
        UserClaims = new HashSet<UserClaim>();
        UserCreators = new HashSet<User>();
        UserModificators = new HashSet<User>();
        UserTokenCreators = new HashSet<UserToken>();
        UserTokenModificators = new HashSet<UserToken>();
        ApplicationCreators = new HashSet<Application>();
        ApplicationModificators = new HashSet<Application>();
        AuthorizationCreators = new HashSet<Authorization>();
        AuthorizationModificators = new HashSet<Authorization>();
        ModuleCreators = new HashSet<Module>();
        ModuleModificators = new HashSet<Module>();
        RoleCreators = new HashSet<Role>();
        RoleModificators = new HashSet<Role>();
        RoleClaimCreators = new HashSet<RoleClaim>();
        RoleClaimModificators = new HashSet<RoleClaim>();
        TokenCreators = new HashSet<Token>();
        TokenModificators = new HashSet<Token>();
        UserClaimCreators = new HashSet<UserClaim>();
        UserClaimModificators = new HashSet<UserClaim>();
        UserLoginCreators = new HashSet<UserLogin>();
        UserLoginModificators = new HashSet<UserLogin>();
        UserRoleCreators = new HashSet<UserRole>();
        UserRoleModificators = new HashSet<UserRole>();
        RegisteredEndpointCreators = new HashSet<RegisteredEndpoint>();
        RegisteredEndpointModificators = new HashSet<RegisteredEndpoint>();
        PermissionCreators = new HashSet<Permission>();
        PermissionModificators = new HashSet<Permission>();
        #endregion
    }

    protected User(string firstName, string lastName, string userName)
    {
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
        Activate();

        #region nav props initialization
        UserRoles = new HashSet<UserRole>();
        UserLogins = new HashSet<UserLogin>();
        UserTokens = new HashSet<UserToken>();
        UserClaims = new HashSet<UserClaim>();
        UserCreators = new HashSet<User>();
        UserModificators = new HashSet<User>();
        UserTokenCreators = new HashSet<UserToken>();
        UserTokenModificators = new HashSet<UserToken>();
        ApplicationCreators = new HashSet<Application>();
        ApplicationModificators = new HashSet<Application>();
        AuthorizationCreators = new HashSet<Authorization>();
        AuthorizationModificators = new HashSet<Authorization>();
        ModuleCreators = new HashSet<Module>();
        ModuleModificators = new HashSet<Module>();
        RoleCreators = new HashSet<Role>();
        RoleModificators = new HashSet<Role>();
        RoleClaimCreators = new HashSet<RoleClaim>();
        RoleClaimModificators = new HashSet<RoleClaim>();
        TokenCreators = new HashSet<Token>();
        TokenModificators = new HashSet<Token>();
        UserClaimCreators = new HashSet<UserClaim>();
        UserClaimModificators = new HashSet<UserClaim>();
        UserLoginCreators = new HashSet<UserLogin>();
        UserLoginModificators = new HashSet<UserLogin>();
        UserRoleCreators = new HashSet<UserRole>();
        UserRoleModificators = new HashSet<UserRole>();
        RegisteredEndpointCreators = new HashSet<RegisteredEndpoint>();
        RegisteredEndpointModificators = new HashSet<RegisteredEndpoint>();
        PermissionCreators = new HashSet<Permission>();
        PermissionModificators = new HashSet<Permission>();
        #endregion
    }

    public UserFirstName FirstName { get; set; }

    public UserLastName LastName { get; set; }

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

    public static User Create(UserFirstName firstName, UserLastName lastName, UserName userName)
        => new(firstName, lastName, userName);

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

    public virtual ICollection<UserToken> UserTokens { get; private set; }

    public virtual ICollection<UserRole> UserRoles { get; private set; }

    public virtual ICollection<UserLogin> UserLogins { get; private set; }

    public virtual ICollection<UserClaim> UserClaims { get; private set; }

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

    public virtual ICollection<Permission> PermissionCreators { get; private set; }

    public virtual ICollection<Permission> PermissionModificators { get; private set; }
    #endregion

}