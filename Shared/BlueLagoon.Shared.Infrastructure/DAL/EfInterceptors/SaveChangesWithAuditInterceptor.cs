using BlueLagoon.Shared.DevTools.Base.Abstractions;
using BlueLagoon.Shared.DevTools.DateAndTime.Abstractions;
using BlueLagoon.Shared.DevTools.Http;
using BlueLagoon.Shared.DevTools.Uniqueidentifier;
using BlueLagoon.Shared.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BlueLagoon.Shared.Infrastructure.DAL.EfInterceptors;

public sealed class SaveChangesWithAuditInterceptor(IDateTimeProvider dateTimeProvider,
                                                    DefaultSystemUser systemUser,
                                                    IHttpContextAccessor httpContextAccessor)
    : SaveChangesInterceptor
{
    private readonly HttpContext httpContext = httpContextAccessor.HttpContext;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        var currentDateTime = dateTimeProvider.Current();
        var userId = httpContext.GetUserIdAsGuid();
        if (userId.IsNullOrEmpty() && (systemUser?.Id.IsNullOrEmpty() ?? true))
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        foreach (var entry in context.ChangeTracker.Entries<IBaseEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.SetCreatorAuditProperties(currentDateTime, userId.IsNullOrEmpty() ? systemUser.Id : Guid.Empty);

            if (entry.State == EntityState.Modified)
                entry.Entity.SetModificatorAuditProperties(currentDateTime, userId.IsNullOrEmpty() ? systemUser.Id : Guid.Empty);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}