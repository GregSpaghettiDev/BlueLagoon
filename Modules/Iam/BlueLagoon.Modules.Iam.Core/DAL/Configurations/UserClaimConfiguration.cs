using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlueLagoon.Shared.DevTools.Base.Abstractions;

namespace BlueLagoon.Modules.Iam.Core.DAL.Configurations;

internal sealed class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.GenerateBasePropertiesRules();

        builder.HasOne(x => x.Creator)
                    .WithMany(x => x.UserClaimCreators)
                    .HasForeignKey(x => x.CreatorId)
                    .IsRequired();

        builder.HasOne(x => x.Modificator)
                    .WithMany(x => x.UserClaimModificators)
                    .HasForeignKey(x => x.ModificatorId)
                    .IsRequired();

        builder.HasOne(x => x.User)
                .WithMany(x => x.UserClaims)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

        builder.HasOne(x => x.Permission)
                .WithMany(x => x.UserClaims)
                .HasForeignKey(x => x.ClaimId)
                .IsRequired(false);

        builder.ToTable("user_claim");
    }
}
