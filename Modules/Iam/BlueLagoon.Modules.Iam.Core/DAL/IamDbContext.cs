using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Shared.DevTools.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlueLagoon.Modules.Iam.Core.DAL;

internal sealed class IamDbContext(DbContextOptions<IamDbContext> options) 
    : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options)
{
    public DbSet<Application> Application { get; set; }

    public DbSet<Authorization> Authorization { get; set; }

    public DbSet<Token> Token { get; set; }

    public DbSet<Module> Module { get; set; }

    public DbSet<RegisteredEndpoint> RegisteredEndpoint { get; set; }

    public DbSet<Permission> Permission { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("iam");
        builder.UseOpenIddict<Application, Authorization, Module, Token, Guid>();
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        foreach (var entity in builder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName()?.ToSnakeCase());

            foreach (var property in entity.GetProperties())
                property.SetColumnName(property.Name.ToSnakeCase());
        }

        builder.RestrictCascadeDelete();
    }
}