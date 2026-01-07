using Common.BaseValueObjects.Abstractions;
using System;

namespace Common.BaseValueObjects
{
    public record BaseDate : IBaseDate, IComparable<BaseDate>
    {
        public virtual DateTime Value { get; private set; }

        public BaseDate(DateTime value)
        {
            Value = value;
        }

        public BaseDate(DateTimeOffset value)
        {
            Value = value.DateTime;
        }

        public BaseDate(int year)
        {
            Value = new DateTime(year, 1, 1, 0, 0, 1);
        }

        public virtual BaseDate AddYears(int years) => new(Value.AddYears(years));

        public virtual BaseDate AddMonths(int months) => new(Value.AddMonths(months));

        public virtual BaseDate AddDays(int days) => new(Value.AddDays(days));

        public virtual BaseDate AddHours(int hours) => new(Value.AddHours(hours));

        public virtual BaseDate AddMinutes(int minutes) => new(Value.AddMinutes(minutes));

        public virtual BaseDate AddSeconds(int second) => new(Value.AddSeconds(second));

        public static TimeSpan operator -(BaseDate date1, BaseDate date2) => date1.Value - date2.Value;

        public static TimeSpan operator +(BaseDate date1, BaseDate date2) => date1.Value - date2.Value;

        public static implicit operator DateTimeOffset(BaseDate date) => date.Value;

        public static implicit operator BaseDate(DateTime dateTime) => new(dateTime);

        public static implicit operator DateTime(BaseDate date) => date.Value;

        public static bool operator <(BaseDate date1, BaseDate date2) => date1.Value < date2.Value;

        public static bool operator >(BaseDate date1, BaseDate date2) => date1.Value > date2.Value;

        public static bool operator <=(BaseDate date1, BaseDate date2) => date1.Value <= date2.Value;

        public static bool operator >=(BaseDate date1, BaseDate date2) => date1.Value >= date2.Value;

        public static bool operator <(DateTime date1, BaseDate date2) => date1 < date2.Value;

        public static bool operator >(DateTime date1, BaseDate date2) => date1 > date2.Value;

        public static bool operator <=(DateTime date1, BaseDate date2) => date1 <= date2.Value;

        public static bool operator >=(DateTime date1, BaseDate date2) => date1 >= date2.Value;

        public static bool operator <(BaseDate date1, DateTime date2) => date1.Value < date2;

        public static bool operator >(BaseDate date1, DateTime date2) => date1.Value > date2;

        public static bool operator <=(BaseDate date1, DateTime date2) => date1.Value <= date2;

        public static bool operator >=(BaseDate date1, DateTime date2) => date1.Value >= date2;

        public static bool operator ==(BaseDate date1, DateTime date2) => date1.Value == date2;

        public static bool operator ==(DateTime date1, BaseDate date2) => date1 == date2.Value;

        public static bool operator !=(BaseDate date1, DateTime date2) => date1.Value != date2;

        public static bool operator !=(DateTime date1, BaseDate date2) => date1 != date2.Value;

        public int CompareTo(BaseDate other)
        {
            if (other == null)
                return 1;

            return DateTime.Compare(Value, other.Value);
        }
    }
}
