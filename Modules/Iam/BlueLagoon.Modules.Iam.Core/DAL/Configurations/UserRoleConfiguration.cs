using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlueLagoon.Modules.Iam.Core.DAL.Configurations;

internal sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.GenerateBasePropertiesRules();

        builder.ToTable("user_role");
    }
}