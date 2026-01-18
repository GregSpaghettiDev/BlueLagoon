using BlueLagoon.Shared.DevTools.Base;
using BlueLagoon.Shared.DevTools.Base.Abstractions;
using BlueLagoon.Shared.DevTools.Base.Exceptions;
using OpenIddict.EntityFrameworkCore.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueLagoon.Modules.Iam.Core.DAL.Entities;

internal class Token : OpenIddictEntityFrameworkCoreToken<BaseId, Application, Authorization>, IBaseEntity
{
    protected Token() { }

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
            throw new InvalidActivationFlagException(nameof(Token));

        if (!IsActive)
            IsActive = true;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidDeactivationFlagException(nameof(Token));

        if (IsActive)
            IsActive = false;
    }

    [InverseProperty(nameof(User.TokenCreators))]
    public virtual User Creator { get; private set; }

    [InverseProperty(nameof(User.TokenModificators))]
    public virtual User Modificator { get; private set; }
}
