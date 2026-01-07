using System;

namespace Common.Date.Abstractions
{
    public interface IDateTimeProvider
    {
        DateTime Current();

        DateTime CurrentUtc();
    }
}
