using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlueLagoon.Shared.DevTools.Base.Abstractions;

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

        builder.ToTable("role_claim");
    }
}
