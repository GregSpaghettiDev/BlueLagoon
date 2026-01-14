using BlueLagoon.Modules.Iam.Core.DAL.Abstractions;
using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Shared.DevTools.Base;
using BlueLagoon.Shared.DevTools.DateAndTime.Abstractions;
using BlueLagoon.Shared.DevTools.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace BlueLagoon.Modules.Iam.Core.DAL;

internal sealed class IamDbContext(DbContextOptions<IamDbContext> options)
    : IdentityDbContext<User, Role, BaseId>(options), IIamDbContext
//: IdentityDbContext<User, Role, BaseId, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options), IIamDbContext
{
    private IDateTimeProvider _dateTimeProvider;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("iam");
        builder.UseOpenIddict();

        foreach (var entity in builder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName()?.ToSnakeCase());

            foreach (var property in entity.GetProperties())
                property.SetColumnName(property.Name.ToSnakeCase());
        }

        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        builder.RestrictCascadeDelete();
    }

    public void SetDbContextFields(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task SaveChangesWithAuditAsync(CancellationToken cancellationToken = default)
    {
        DateTime currentUtc = _dateTimeProvider.Current();

        //foreach (EntityEntry<IBaseEntity> entry in ChangeTracker.Entries<IBaseEntity>())
        //{
            //if (entry.State == EntityState.Added)
            //    entry.Entity.SetCreatorAuditProperties(currentUtc,
            //                                           entry.Entity.CreatorId?.Value.IsNullOrEmpty() ?? true
            //                                                        ? (setUserId ? _userContext.UserId : _systemUser.Id)
            //                                                        : entry.Entity.CreatorId);

            //if (entry.State == EntityState.Modified)
            //    entry.Entity.SetModificatorAuditProperties(currentUtc,
            //                                               entry.Entity.ModificatorId?.Value.IsNullOrEmpty() ?? true
            //                                                        ? (setUserId ? _userContext.UserId : _systemUser.Id)
            //                                                        : entry.Entity.ModificatorId);
        //}

        await base.SaveChangesAsync(cancellationToken);
    }
}