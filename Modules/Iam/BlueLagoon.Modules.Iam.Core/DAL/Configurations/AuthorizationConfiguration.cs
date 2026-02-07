using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Shared.DevTools.Base;
using BlueLagoon.Shared.DevTools.Base.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlueLagoon.Modules.Iam.Core.DAL.Configurations;

internal class Authorizationfiguration : IEntityTypeConfiguration<Authorization>
{
    public void Configure(EntityTypeBuilder<Authorization> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
                    .IsRequired();

        builder.GenerateBasePropertiesRules();

        builder.HasOne(x => x.Creator)
                    .WithMany(x => x.AuthorizationCreators)
                    .HasForeignKey(x => x.CreatorId)
                    .IsRequired();

        builder.HasOne(x => x.Modificator)
                    .WithMany(x => x.AuthorizationModificators)
                    .HasForeignKey(x => x.ModificatorId)
                    .IsRequired();

        builder.ToTable("authorization");
    }
}
