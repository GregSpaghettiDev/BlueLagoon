using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlueLagoon.Modules.Iam.Core.DAL.Configurations;

internal sealed class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.GenerateBasePropertiesRules();

        builder.ToTable("user_claim");
    }
}
