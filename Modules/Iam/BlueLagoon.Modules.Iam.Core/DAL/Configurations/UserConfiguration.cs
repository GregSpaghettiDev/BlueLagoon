using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Shared.DevTools.Base;
using BlueLagoon.Shared.DevTools.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlueLagoon.Modules.Iam.Core.DAL.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
                    .HasConversion(x => x.Value, x => new BaseId(x))
                    .IsRequired();

        builder.GenerateBasePropertiesRules();

        builder.ComplexProperty(x => x.FirstName, cpb =>
        {
            cpb.Property(x => x.Value)
                .HasColumnName(nameof(User.FirstName).ToSnakeCase())
                .HasMaxLength(60)
                .IsRequired();

            cpb.IsRequired();
        });

        builder.ComplexProperty(x => x.LastName, cpb =>
        {
            cpb.Property(x => x.Value)
                .HasColumnName(nameof(User.LastName).ToSnakeCase())
                .HasMaxLength(60)
                .IsRequired();

            cpb.IsRequired();
        });
    }
}
