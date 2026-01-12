using BlueLagoon.Shared.DevTools.Base;
using BlueLagoon.Shared.DevTools.Base.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace BlueLagoon.Modules.Iam.Core.DAL.Entities;

internal class UserRole : IdentityUserRole<BaseId>, IBaseEntity
{
    public BaseEntity MetaData { get; set; } = new();

    public BaseDate CreatedAt => MetaData.CreatedAt;

    public BaseId CreatorId => MetaData.CreatorId;

    public BaseDate ModifiedAt => MetaData?.ModifiedAt;

    public BaseId ModificatorId => MetaData?.ModificatorId;

    public bool IsActive => MetaData.IsActive;
}