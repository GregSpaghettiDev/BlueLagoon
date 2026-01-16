using BlueLagoon.Shared.DevTools.Base;
using BlueLagoon.Shared.DevTools.Base.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlueLagoon.Shared.Infrastructure.DAL;

public static class Extensions
{
    public static void GenerateBasePropertiesRules<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseEntity
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
                    .HasConversion(x => x.Value, x => new BaseId(x))
                    .IsRequired();

        builder.Property(x => x.CreatedAt)
                    .HasColumnType("datetime2(0)")
                    .HasConversion(x => x.Value, x => new BaseDate(x))
                    .IsRequired();

        builder.Property(x => x.CreatorId)
                    .HasConversion(x => x.Value, x => new BaseId(x))
                    .IsRequired();

        builder.Property(x => x.ModifiedAt)
                    .HasColumnType("datetime2(0)")
                    .HasConversion(x => x.Value, x => new BaseDate(x))
                    .IsRequired(false);

        builder.Property(x => x.ModificatorId)
                    .HasConversion(x => x.Value, x => new BaseId(x))
                    .IsRequired(false);

        builder.Property(x => x.IsActive)
                    .IsRequired();

        builder.Property<uint>("xmin")
               .IsRowVersion();
    }
}