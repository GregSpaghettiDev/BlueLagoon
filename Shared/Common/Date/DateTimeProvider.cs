using Common.Date.Abstractions;
using System;

namespace Common.Date
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Current() => DateTime.UtcNow.ConvertDate();

        public DateTime CurrentUtc() => DateTime.UtcNow;
    }
}
