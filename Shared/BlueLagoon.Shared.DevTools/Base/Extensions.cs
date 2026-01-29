using BlueLagoon.Shared.DevTools.Base.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlueLagoon.Shared.DevTools.Base;

public static class Extensions
{
    public static void GenerateBasePropertiesRules<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseEntity<TEntity>
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
                    .IsRequired();

        builder.Property(x => x.CreatedAt)
                    .HasColumnType("timestamp(0)")
                    .IsRequired();

        builder.Property(x => x.CreatorId)
                    .IsRequired();

        builder.Property(x => x.ModifiedAt)
                    .HasColumnType("timestamp(0)")
                    .IsRequired(false);

        builder.Property(x => x.ModificatorId)
                    .IsRequired(false);

        builder.Property(x => x.IsActive)
                    .IsRequired()
                    .HasDefaultValue(true);

        builder.Property<uint>("xmin")
               .IsRowVersion();
    }
}