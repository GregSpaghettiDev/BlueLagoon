//using BlueLagoon.Modules.Iam.Core.DAL.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using BlueLagoon.Shared.DevTools.Base.Abstractions;

//namespace BlueLagoon.Modules.Iam.Core.DAL.Configurations;

//internal sealed class RegisteredEndpointRoleConfiguration : IEntityTypeConfiguration<RegisteredEndpointRole>
//{
//    public void Configure(EntityTypeBuilder<RegisteredEndpointRole> builder)
//    {
//        builder.HasKey(x => new
//        {
//            x.RegisteredEndpointId,
//            x.RoleId
//        });

//        builder.GenerateBasePropertiesRules();

//        builder.HasOne(x => x.RegisteredEndpoint)
//                    .WithMany(x => x.RegisteredEndpointRoles)
//                    .HasForeignKey(x => x.RegisteredEndpointId)
//                    .IsRequired();

//        builder.HasOne(x => x.Role)
//                    .WithMany(x => x.RegisteredEndpointRoles)
//                    .HasForeignKey(x => x.RoleId)
//                    .IsRequired();

//        builder.HasOne(x => x.Creator)
//                    .WithMany(x => x.RegisteredEndpointRoleCreators)
//                    .HasForeignKey(x => x.CreatorId)
//                    .IsRequired();

//        builder.HasOne(x => x.Modificator)
//                    .WithMany(x => x.RegisteredEndpointRoleModificators)
//                    .HasForeignKey(x => x.ModificatorId)
//                    .IsRequired();

//        builder.ToTable("registered_endpoint_role");
//    }
//}

