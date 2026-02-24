using AutoMapper;
using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Modules.Iam.Core.Services.Dto;

namespace BlueLagoon.Modules.Iam.Core.Mappings;

internal sealed class IamProfile : Profile
{
    public IamProfile()
    {
        CreateMap<Module, ModuleDto>()
            .ForMember(d => d.Code, o => o.MapFrom(s => s.Name))
            .ForMember(d => d.CreatorName, o => o.MapFrom(s => s.Creator != null ? s.Creator.FirstName + " " + s.Creator.LastName : string.Empty))
            .ForMember(d => d.ModificatorName, o => o.MapFrom(s => s.Modificator != null ? s.Modificator.FirstName + " " + s.Modificator.LastName : string.Empty))
            .ForMember(d => d.Description, o => o.MapFrom(s => s.DisplayName))
            .ForMember(d => d.Options, o => o.MapFrom(s => new ModuleDtoOptionsDto
            {
                IsDeactivate = s.IsActive
            }));

        CreateMap<Role, RoleDto>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.DisplayName))
            .ForMember(d => d.Code, o => o.MapFrom(s => s.Name))
            .ForMember(d => d.CreatorName, o => o.MapFrom(s => s.Creator != null ? s.Creator.FirstName + " " + s.Creator.LastName : string.Empty))
            .ForMember(d => d.Options, o => o.MapFrom(s => new RoleOptionsDto
            {
                IsDetails = true,
                IsActivate = !s.IsActive,
                IsDelete = s.IsActive,
            }));

        CreateMap<Role, RoleWithPermissionsDto>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.DisplayName))
            .ForMember(d => d.Code, o => o.MapFrom(s => s.Name))
            .ForMember(d => d.CreatorName, o => o.MapFrom(s => s.Creator != null ? s.Creator.FirstName + " " + s.Creator.LastName : string.Empty))
            .ForMember(d => d.AssignedPermissions, o => o.MapFrom(s => s.RoleClaims))
            .ForMember(d => d.AvailablePermissions, o => o.Ignore())
            .ForMember(d => d.Options, o => o.MapFrom(s => new RoleOptionsDto
            {
                IsDetails = true,
                IsActivate = !s.IsActive,
                IsDelete = s.IsActive,
            }));

        CreateMap<RoleClaim, ClaimDto>()
            .ForMember(d => d.Code, o => o.MapFrom(s => s.ClaimValue))
            .ForMember(d => d.Description, o => o.MapFrom(s => s.ClaimDescription))
            .ForMember(d => d.CreatorName, o => o.MapFrom(s => s.Creator != null ? s.Creator.FirstName + " " + s.Creator.LastName : string.Empty))
            .ForMember(d => d.Type, o => o.MapFrom(s => s.ClaimType))
            .ForMember(d => d.ModuleName, o => o.MapFrom(s => s.ModuleName));

        CreateMap<Permission, ClaimDto>()
            .ForMember(d => d.Code, o => o.MapFrom(s => s.FullPermissionName.Name))
            .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
            .ForMember(d => d.CreatorName, o => o.MapFrom(s => s.Creator != null ? s.Creator.FirstName + " " + s.Creator.LastName : string.Empty))
            .ForMember(d => d.ModuleName, o => o.MapFrom(s => s.FullPermissionName.ModuleName));

        CreateMap<Permission, PermissionDto>()
            .ForMember(d => d.Code, o => o.MapFrom(s => s.FullPermissionName.ModuleName + "." + s.FullPermissionName.Name))
            .ForMember(d => d.CreatorName, o => o.MapFrom(s => s.Creator != null ? s.Creator.FirstName + " " + s.Creator.LastName : string.Empty))
            .ForMember(d => d.Options, o => o.MapFrom(s => new PermissionOptionsDto
            {
                IsDetails = true,
                IsActivate = !s.IsActive,
                IsDelete = s.IsActive
            }));

        CreateMap<User, UserDto>()
            .ForMember(d => d.CreatorName, o => o.MapFrom(s => s.Creator != null ? s.Creator.FirstName + " " + s.Creator.LastName : string.Empty))
            .ForMember(d => d.AssignedModules, o => o.MapFrom(s => string.Join(", ", s.UserClaims
                    .Select(uc => uc.ModuleName)
                    .Concat(s.UserRoles.SelectMany(ur => ur.Role.RoleClaims).Select(rc => rc.ModuleName))
                    .Distinct())))
            .ForMember(d => d.Options, o => o.MapFrom(s => new UserOptionsDto
            {
                IsActivate = !s.IsActive,
                IsDetails = true
            }));

        CreateMap<User, BaseUserDto>()
            .ForMember(d => d.CreatorName, o => o.MapFrom(s => s.Creator != null ? s.Creator.FirstName + " " + s.Creator.LastName : string.Empty))
            .ForMember(d => d.Options, o => o.MapFrom(s => new UserOptionsDto
            {
                IsActivate = !s.IsActive,
                IsDetails = true
            }));

        CreateMap<RoleClaim, UserPermissionDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.IsFromRole, o => o.MapFrom(_ => true))
            .ForMember(d => d.Code, o => o.MapFrom(s => s.ClaimValue))
            .ForMember(d => d.ModuleName, o => o.MapFrom(s => s.ModuleName))
            .ForMember(d => d.Description, o => o.MapFrom(s => s.ClaimDescription))
            .ForMember(d => d.RoleId, o => o.MapFrom(s => s.RoleId))
            .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role.Name));

        CreateMap<UserClaim, UserPermissionDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.IsFromRole, o => o.MapFrom(_ => false))
            .ForMember(d => d.Code, o => o.MapFrom(s => s.ClaimValue))
            .ForMember(d => d.ModuleName, o => o.MapFrom(s => s.ModuleName))
            .ForMember(d => d.Description, o => o.MapFrom(s => s.ClaimDescription))
            .ForMember(d => d.RoleId, o => o.MapFrom(_ => (Guid?)null))
            .ForMember(d => d.RoleName, o => o.MapFrom(_ => (string)null));

        CreateMap<UserRole, UserRoleDto>()
            .ForMember(d => d.RoleId, o => o.MapFrom(s => s.RoleId))
            .ForMember(d => d.RoleCode, o => o.MapFrom(s => s.Role.Name))
            .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role.DisplayName))
            .ForMember(d => d.AccesingModules, o => o.MapFrom(s => string.Join(", ", s.Role.RoleClaims.Select(x => x.ModuleName))));

        CreateMap<RegisteredEndpoint, RegisteredEndpointDto>()
            .ForMember(d => d.ModuleName, o => o.MapFrom(s => s.ModuleName))
            .ForMember(d => d.Path, o => o.MapFrom(s => s.Path))
            .ForMember(d => d.OperationId, o => o.MapFrom(s => s.OperationId))
            .ForMember(d => d.CreatorName, o => o.MapFrom(s => s.Creator != null ? s.Creator.FirstName + " " + s.Creator.LastName : string.Empty))
            .ForMember(d => d.ModificatorName, o => o.MapFrom(s => s.Modificator != null ? s.Modificator.FirstName + " " + s.Modificator.LastName : string.Empty))
            .ForMember(d => d.HasPermissionMappings, o => o.MapFrom(s => s.RegisteredEndpointPermissions.Count > 0));

        CreateMap<RegisteredEndpoint, RegisteredEndpointDto>()
            .ForMember(d => d.ModuleName, o => o.MapFrom(s => s.ModuleName))
            .ForMember(d => d.Path, o => o.MapFrom(s => s.Path))
            .ForMember(d => d.OperationId, o => o.MapFrom(s => s.OperationId))
            .ForMember(d => d.CreatorName, o => o.MapFrom(s => s.Creator != null ? s.Creator.FirstName + " " + s.Creator.LastName : string.Empty))
            .ForMember(d => d.ModificatorName, o => o.MapFrom(s => s.Modificator != null ? s.Modificator.FirstName + " " + s.Modificator.LastName : string.Empty))
            .ForMember(d => d.HasPermissionMappings, o => o.MapFrom(s => s.RegisteredEndpointPermissions.Count > 0));

        CreateMap<RegisteredEndpointPermission, EndpointRequirementDto>()
            .ForMember(d => d.RegisteredEndpointId, o => o.MapFrom(s => s.RegisteredEndpointId))
            .ForMember(d => d.PermissionId, o => o.MapFrom(s => s.PermissionId))
            .ForMember(d => d.Code, o => o.MapFrom(s => s.Permission.FullPermissionName.Name))
            .ForMember(d => d.ModuleName, o => o.MapFrom(s => s.Permission.FullPermissionName.ModuleName))
            .ForMember(d => d.Description, o => o.MapFrom(s => s.Permission.Description));

        CreateMap<Permission, EndpointRequirementDto>()
            .ForMember(d => d.RegisteredEndpointId, o => o.MapFrom(s => (Guid?)null))
            .ForMember(d => d.PermissionId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Code, o => o.MapFrom(s => s.FullPermissionName.Name))
            .ForMember(d => d.ModuleName, o => o.MapFrom(s => s.FullPermissionName.ModuleName))
            .ForMember(d => d.Description, o => o.MapFrom(s => s.Description));
    }
}