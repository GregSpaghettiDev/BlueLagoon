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
                    .HasConversion(x => x.Value, x => new BaseDate(x))
                    .IsRequired();

        builder.Property(x => x.CreatorId)
                    .HasConversion(x => x.Value, x => new BaseId(x))
                    .IsRequired();

        builder.Property(x => x.ModifiedAt)
                    .HasColumnType("timestamp(0)")
                    .HasConversion(x => x == null ? (DateTime?)null : x.Value, x => x != null ? new BaseDate(x.Value) : null)
                    .IsRequired(false);

        builder.Property(x => x.ModificatorId)
                    .HasConversion(x => x == null ? (Guid?)null : x.Value, x => x != null ? new BaseId(x.Value) : null)
                    .IsRequired(false);

        builder.Property(x => x.IsActive)
                    .IsRequired()
                    .HasDefaultValue(true);

        builder.Property<uint>("xmin")
               .IsRowVersion();
    }
}
