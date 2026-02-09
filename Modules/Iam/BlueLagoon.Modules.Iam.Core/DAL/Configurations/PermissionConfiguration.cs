using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Shared.DevTools.Base;
using BlueLagoon.Shared.DevTools.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlueLagoon.Modules.Iam.Core.DAL.Configurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.GenerateBasePropertiesRules();

        builder.ComplexProperty(x => x.FullPermissionName, cpb =>
        {
            cpb.Property(x => x.Name)
                .HasColumnName(nameof(Permission.FullPermissionName.Name).ToSnakeCase())
                .HasMaxLength(256)
                .IsRequired();
            
            cpb.Property(x => x.ModuleName)
                .HasColumnName(nameof(Permission.FullPermissionName.ModuleName).ToSnakeCase())
                .HasMaxLength(256)
                .IsRequired();

            cpb.Ignore(x => x.FullPermissionName);

            cpb.IsRequired();
        });

        builder.Property(x => x.Description)
            .HasColumnName(nameof(Permission.Description).ToSnakeCase())
            .HasMaxLength(512)
            .IsRequired(false);

        builder.HasOne(x => x.Creator)
                    .WithMany(x => x.PermissionCreators)
                    .HasForeignKey(x => x.CreatorId)
                    .IsRequired();

        builder.HasOne(x => x.Modificator)
                    .WithMany(x => x.PermissionModificators)
                    .HasForeignKey(x => x.ModificatorId)
                    .IsRequired();
    }
}
