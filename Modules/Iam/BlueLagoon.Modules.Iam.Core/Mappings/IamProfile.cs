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
    }
}