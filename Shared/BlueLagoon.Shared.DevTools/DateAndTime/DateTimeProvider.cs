using BlueLagoon.Shared.DevTools.DateAndTime.Abstractions;

namespace BlueLagoon.Shared.DevTools.DateAndTime;

internal class DateTimeProvider : IDateTimeProvider
{
    public DateTime Current() => DateTime.UtcNow.ConvertDate();

    public DateTime CurrentUtc() => DateTime.UtcNow;
}