using System.Collections.Generic;
using System.Dynamic;

namespace Common.Dynamic.Extensions
{
    public static class ExpandoExtension
    {
        public static void AddProperty(this ExpandoObject expando, string propertyName, object propertyValue)
        {
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
    }
}
