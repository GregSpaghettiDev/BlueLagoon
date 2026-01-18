using AutoMapper;
using BlueLagoon.Shared.DevTools.Base;

namespace BlueLagoon.Shared.Infrastructure.Mappings;

internal sealed class BaseProfile : Profile
{
    public BaseProfile()
    {
        CreateMap<BaseId, Guid>()
            .ConvertUsing(x => x.Value);

        CreateMap<Guid, BaseId>()
            .ConvertUsing(x => new BaseId(x));

        CreateMap<BaseDate, DateTime>()
            .ConvertUsing(x => x.Value);

        CreateMap<DateTime, BaseDate>()
            .ConvertUsing(x => new BaseDate(x));

        CreateMap<BaseId, Guid?>()
            .ConvertUsing(x => x != null ? x.Value : null);

        CreateMap<BaseDate, DateTime?>()
            .ConvertUsing(x => x != null ? x.Value : null);
    }
}
