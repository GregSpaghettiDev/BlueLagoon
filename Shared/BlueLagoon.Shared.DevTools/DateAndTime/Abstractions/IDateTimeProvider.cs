namespace BlueLagoon.Shared.DevTools.DateAndTime.Abstractions;

public interface IDateTimeProvider
{
    DateTime Current();

    DateTime CurrentUtc();
}