using BlueLagoon.Modules.Iam.Core.DAL.Abstractions;
using BlueLagoon.Shared.DevTools.Base;
using BlueLagoon.Shared.DevTools.Base.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlueLagoon.Modules.Iam.Core.DAL;

internal static class Extensions
{
    public static void GenerateBasePropertiesRules<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IBaseEntity
    {
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
