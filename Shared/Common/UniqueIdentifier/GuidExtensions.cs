using Common.BaseValueObjects;
using System;

namespace Common.UniqueIdentifier
{
    public static class GuidExtensions
    {
        public static bool IsNullOrEmpty(this Guid? guid)
        {
            return guid is null || guid == Guid.Empty;
        }

        public static bool IsNullOrEmpty(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        public static BaseId ToBaseId(this Guid uniqueIdentifier) => new(uniqueIdentifier);

        public static BaseId ToBaseId(this Guid? uniqueIdentifier) => uniqueIdentifier.IsNullOrEmpty() ? null : new BaseId((Guid)uniqueIdentifier);
    }
}
