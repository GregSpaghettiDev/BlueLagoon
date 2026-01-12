using BlueLagoon.Shared.DevTools.Base;

namespace BlueLagoon.Shared.DevTools.Uniqueidentifier;

public static class Extensions
{
    public static bool IsNullOrEmpty(this Guid? guid)
        => guid is null || guid == Guid.Empty;

    public static bool IsNullOrEmpty(this Guid guid)
        => guid == Guid.Empty;


    public static BaseId ToBaseId(this Guid uniqueIdentifier) 
        => new(uniqueIdentifier);

    public static BaseId ToBaseId(this Guid? uniqueIdentifier) 
        => uniqueIdentifier.IsNullOrEmpty() ? null : new BaseId((Guid)uniqueIdentifier);
}
