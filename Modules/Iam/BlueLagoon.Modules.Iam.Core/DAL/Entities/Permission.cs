using BlueLagoon.Modules.Iam.Core.ValueObjects;
using BlueLagoon.Shared.DevTools.Base.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueLagoon.Modules.Iam.Core.DAL.Entities;

internal class Permission : BaseEntity<Permission>
{
    protected Permission()
    {
        Activate();
        RoleClaims = new HashSet<RoleClaim>();
        UserClaims = new HashSet<UserClaim>();
    }

    private Permission(PermissionName name, string description)
    {
        Activate();
        RoleClaims = new HashSet<RoleClaim>();
        UserClaims = new HashSet<UserClaim>();
        FullPermissionName = name;
        Description = description;
        Id = Guid.NewGuid();
    }

    public PermissionName FullPermissionName { get; private set; }

    public string Description { get; private set; }

    public static Permission Create(PermissionName name, string description)
        => new(name, description);

    public void ChangeDescription(string description)
        => Description = description; 

    [InverseProperty(nameof(User.PermissionCreators))]
    public virtual User Creator { get; private set; }

    [InverseProperty(nameof(User.PermissionModificators))]
    public virtual User Modificator { get; private set; }

    public virtual ICollection<RoleClaim> RoleClaims { get; private set; }

    public virtual ICollection<UserClaim> UserClaims { get; private set; }
}
