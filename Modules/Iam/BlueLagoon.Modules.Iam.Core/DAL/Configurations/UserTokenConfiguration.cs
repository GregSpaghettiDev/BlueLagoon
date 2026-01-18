using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlueLagoon.Shared.DevTools.Base.Abstractions;

namespace BlueLagoon.Modules.Iam.Core.DAL.Configurations;

internal sealed class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.GenerateBasePropertiesRules();

        builder.HasOne(x => x.Creator)
                    .WithMany(x => x.UserTokenCreators)
                    .HasForeignKey(x => x.CreatorId)
                    .IsRequired();

        builder.HasOne(x => x.Modificator)
                    .WithMany(x => x.UserTokenModificators)
                    .HasForeignKey(x => x.ModificatorId)
                    .IsRequired();

        builder.ToTable("user_token");
    }
}