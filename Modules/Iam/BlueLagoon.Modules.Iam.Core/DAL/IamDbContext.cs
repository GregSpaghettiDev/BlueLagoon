using BlueLagoon.Modules.Iam.Core.DAL.Abstractions;
using BlueLagoon.Shared.DevTools.DateAndTime.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BlueLagoon.Modules.Iam.Core.DAL;

internal sealed class IamDbContext(DbContextOptions options) : DbContext(options), IIamDbContext
{
    private IDateTimeProvider _dateTimeProvider;

    public void SetDbContextFields(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }
}