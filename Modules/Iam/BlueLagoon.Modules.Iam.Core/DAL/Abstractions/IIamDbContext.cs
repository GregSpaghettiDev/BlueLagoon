using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Shared.Infrastructure.DAL.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BlueLagoon.Modules.Iam.Core.DAL.Abstractions;

interface IIamDbContext : IDbContextPool
{
    DbSet<User> Users { get; }

    DbSet<Role> Roles { get; }

    DbSet<UserRole> UserRoles { get; }

    DbSet<UserClaim> UserClaims { get; }

    DbSet<UserLogin> UserLogins { get; }

    DbSet<RoleClaim> RoleClaims { get; }

    DbSet<UserToken> UserTokens { get; }
}
