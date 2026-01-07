using System;

namespace Common.BaseValueObjects.Abstractions
{
    public interface IBaseDate
    {
        DateTime Value { get; }

        BaseDate AddYears(int years);

        BaseDate AddMonths(int months);

        BaseDate AddDays(int days);

        BaseDate AddHours(int hours);

        BaseDate AddMinutes(int minutes);

        BaseDate AddSeconds(int second);
    }
}
