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
    }
}