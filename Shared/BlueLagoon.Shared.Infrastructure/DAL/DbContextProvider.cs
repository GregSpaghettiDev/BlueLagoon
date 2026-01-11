using BlueLagoon.Shared.DevTools.DateAndTime.Abstractions;
using BlueLagoon.Shared.Infrastructure.DAL.Abstractions;

namespace BlueLagoon.Shared.Infrastructure.DAL;

public class DbContextProvider<TDbContext> where TDbContext : IDbContextPool
{
    public readonly TDbContext DbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DbContextProvider(TDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        DbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;

        DbContext.SetDbContextFields(_dateTimeProvider);
    }
}
