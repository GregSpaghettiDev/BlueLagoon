using BlueLagoon.Modules.Iam.Core.ValueObjects;
using BlueLagoon.Shared.DevTools.Base.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueLagoon.Modules.Iam.Core.DAL.Entities;

internal class Permission : BaseEntity<Permission>
{
    public Permission()
    {
        Activate();
    }

    public PermissionName FullPermissionName { get; init; }

    public string Description { get; init; }

    [InverseProperty(nameof(User.PermissionCreators))]
    public virtual User Creator { get; private set; }

    [InverseProperty(nameof(User.PermissionModificators))]
    public virtual User Modificator { get; private set; }

    public virtual ICollection<RoleClaim> RoleClaims { get; private set; }

    public virtual ICollection<UserClaim> UserClaims { get; private set; }
}
