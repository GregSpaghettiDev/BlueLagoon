using BlueLagoon.Shared.DevTools.DateAndTime.Abstractions;

namespace BlueLagoon.Shared.Infrastructure.DAL.Abstractions;

public interface IDbContextPool
{
    void SetDbContextFields(IDateTimeProvider dateTimeProvider);
}