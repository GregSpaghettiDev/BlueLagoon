using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BlueLagoon.Modules.Iam.Core.DAL.Identity;

internal sealed class IamUserStore : UserStore<User, Role, IamDbContext, Guid>
{
    public IamUserStore(IamDbContext context, IdentityErrorDescriber errorDescriber = null)
        : base(context, errorDescriber)
    {
        AutoSaveChanges = true;
    }
}
