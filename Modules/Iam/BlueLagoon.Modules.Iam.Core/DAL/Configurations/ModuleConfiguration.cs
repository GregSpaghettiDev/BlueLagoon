using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Shared.DevTools.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlueLagoon.Modules.Iam.Core.DAL.Configurations;

internal class ModuleConfiguration : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
                    .HasConversion(x => x.Value, x => new BaseId(x))
                    .IsRequired();

        builder.GenerateBasePropertiesRules();

        builder.ToTable("module");
    }
}
