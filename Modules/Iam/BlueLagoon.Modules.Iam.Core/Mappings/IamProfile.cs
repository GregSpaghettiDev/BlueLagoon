using AutoMapper;
using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Base;

namespace BlueLagoon.Modules.Iam.Core.Mappings;

internal sealed class IamProfile : Profile
{
    public IamProfile()
    {
        CreateMap<Module, ModuleDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id.Value))
            .ForMember(d => d.Code, o => o.MapFrom(s => s.Name))
            .ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedAt.Value))
            .ForMember(d => d.CreatorId, o => o.MapFrom(s => s.CreatorId.Value))
            .ForMember(d => d.Description, o => o.MapFrom(s => s.DisplayName))
            .ForMember(d => d.Options, o => o.MapFrom(s => new ModuleDtoOptionsDto
            {
                IsDeactivate = s.IsActive
            }));
    }
}