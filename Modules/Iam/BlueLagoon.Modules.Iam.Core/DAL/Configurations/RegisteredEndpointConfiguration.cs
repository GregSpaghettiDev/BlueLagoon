using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Modules.Iam.Core.ValueObjects;
using BlueLagoon.Shared.DevTools.Base;
using BlueLagoon.Shared.DevTools.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlueLagoon.Modules.Iam.Core.DAL.Configurations;

internal sealed class RegisteredEndpointConfiguration : IEntityTypeConfiguration<RegisteredEndpoint>
{
    public void Configure(EntityTypeBuilder<RegisteredEndpoint> builder)
    {
        builder.GenerateBasePropertiesRules();

        builder.ComplexProperty(x => x.OperationId, cpb =>
        {
            cpb.Property(x => x.Value)
                .HasColumnName(nameof(RegisteredEndpoint.OperationId).ToSnakeCase())
                .HasMaxLength(OperationId.MaxCharactersNumber)
                .IsRequired();
            cpb.IsRequired();
        });

        builder.ComplexProperty(x => x.ModuleName, cpb =>
        {
            cpb.Property(x => x.Value)
                .HasColumnName(nameof(RegisteredEndpoint.ModuleName).ToSnakeCase())
                .HasMaxLength(ModuleName.MaxCharactersNumber)
                .IsRequired();
            cpb.IsRequired();
        });

        builder.ComplexProperty(x => x.Path, cpb =>
        {
            cpb.Property(x => x.Value)
                .HasColumnName(nameof(RegisteredEndpoint.Path).ToSnakeCase())
                .HasMaxLength(200)
                .IsRequired();
            cpb.IsRequired();
        });

        builder.Property(x => x.HttpMethod)
                .HasColumnName(nameof(RegisteredEndpoint.HttpMethod).ToSnakeCase())
                .HasConversion(x => x.Method, x => new HttpMethod(x))
                .IsRequired();

        builder.ComplexProperty(x => x.OperationDescription, cpb =>
        {
            cpb.Property(x => x.Value)
                .HasColumnName(nameof(RegisteredEndpoint.OperationDescription).ToSnakeCase())
                .HasMaxLength(500)
                .IsRequired();
            cpb.IsRequired(false);
        });
    }
}
