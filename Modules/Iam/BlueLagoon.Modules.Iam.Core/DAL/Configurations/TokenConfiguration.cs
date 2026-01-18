using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Shared.DevTools.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlueLagoon.Shared.DevTools.Base.Abstractions;

namespace BlueLagoon.Modules.Iam.Core.DAL.Configurations;

internal class TokenConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
                    .HasConversion(x => x.Value, x => new BaseId(x))
                    .IsRequired();

        builder.GenerateBasePropertiesRules();

        builder.HasOne(x => x.Creator)
                    .WithMany(x => x.TokenCreators)
                    .HasForeignKey(x => x.CreatorId)
                    .IsRequired();

        builder.HasOne(x => x.Modificator)
                    .WithMany(x => x.TokenModificators)
                    .HasForeignKey(x => x.ModificatorId)
                    .IsRequired();

        builder.ToTable("token");
    }
}