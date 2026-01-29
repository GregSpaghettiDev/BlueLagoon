using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BlueLagoon.Shared.DevTools.Base.Abstractions;

public static class Extensions
{
    public static void GenerateBasePropertiesRules<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IBaseEntity
    {
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
