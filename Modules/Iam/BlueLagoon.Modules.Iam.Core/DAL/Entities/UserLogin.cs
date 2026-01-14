using BlueLagoon.Shared.DevTools.Base;
using BlueLagoon.Shared.DevTools.Base.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace BlueLagoon.Modules.Iam.Core.DAL.Entities;

internal class UserLogin : IdentityUserLogin<BaseId>, IBaseEntity
{
    public BaseDate CreatedAt { get; private set; }
    public BaseId CreatorId { get; private set; }

    public BaseDate ModifiedAt { get; private set; }

    public BaseId ModificatorId { get; private set; }

    public bool IsActive { get; private set; }
}