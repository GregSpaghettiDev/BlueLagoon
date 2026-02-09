using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Shared.DevTools.Base.Abstractions;
using BlueLagoon.Shared.DevTools.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlueLagoon.Modules.Iam.Core.DAL.Configurations;

internal sealed class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.GenerateBasePropertiesRules();

        builder.HasOne(x => x.Creator)
                    .WithMany(x => x.RoleClaimCreators)
                    .HasForeignKey(x => x.CreatorId)
                    .IsRequired();

        builder.HasOne(x => x.Modificator)
                    .WithMany(x => x.RoleClaimModificators)
                    .HasForeignKey(x => x.ModificatorId)
                    .IsRequired();

        builder.Property(x => x.ModuleName)
                .HasColumnName(nameof(RoleClaim.ModuleName).ToSnakeCase())
                .HasMaxLength(256)
                .IsRequired();

        builder.Property(x => x.ClaimDescription)
            .HasColumnName(nameof(RoleClaim.ClaimDescription).ToSnakeCase())
            .HasMaxLength(512)
            .IsRequired(false);

        builder.HasOne(x => x.Role)
                .WithMany(x => x.RoleClaims)
                .HasForeignKey(x => x.RoleId)
                .IsRequired();

        builder.HasOne(x => x.Permission)
                .WithMany(x => x.RoleClaims)
                .HasForeignKey(x => x.ClaimId)
                .IsRequired(false);

        builder.ToTable("role_claim");
    }
}
