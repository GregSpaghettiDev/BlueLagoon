using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlueLagoon.Shared.DevTools.Base.Abstractions;

namespace BlueLagoon.Modules.Iam.Core.DAL.Configurations;

internal sealed class RegisteredEndpointPermissionConfiguration : IEntityTypeConfiguration<RegisteredEndpointPermission>
{
    public void Configure(EntityTypeBuilder<RegisteredEndpointPermission> builder)
    {
        builder.HasKey(x => new
        {
            x.RegisteredEndpointId,
            x.PermissionId
        });

        builder.GenerateBasePropertiesRules();

        builder.HasOne(x => x.RegisteredEndpoint)
                    .WithMany(x => x.RegisteredEndpointPermissions)
                    .HasForeignKey(x => x.RegisteredEndpointId)
                    .IsRequired();

        builder.HasOne(x => x.Permission)
                    .WithMany(x => x.RegisteredEndpointPermissions)
                    .HasForeignKey(x => x.PermissionId)
                    .IsRequired();

        builder.HasOne(x => x.Creator)
                    .WithMany(x => x.RegisteredEndpointPermissionCreators)
                    .HasForeignKey(x => x.CreatorId)
                    .IsRequired();

        builder.HasOne(x => x.Modificator)
                    .WithMany(x => x.RegisteredEndpointPermissionModificators)
                    .HasForeignKey(x => x.ModificatorId)
                    .IsRequired();

        builder.ToTable("registered_endpoint_permission");
    }
}
