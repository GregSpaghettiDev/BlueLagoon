using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Common.JsonTypeResolver;

public class PrivateConstructorContractResolver : DefaultJsonTypeInfoResolver
{
    /// <summary>
    /// Works with .NET 7
    /// </summary>
    /// <param name="type"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        if (jsonTypeInfo.Kind == (JsonTypeInfoKind.Object | JsonTypeInfoKind.Enumerable) && jsonTypeInfo.CreateObject is null)
        {
            //if (jsonTypeInfo.Type.GetConstructors((BindingFlags.Public && BindingFlags. )  | BindingFlags.Instance).Length == 0)
            //{
            //The type doesn't have public constructors
            jsonTypeInfo.CreateObject = () =>
                Activator.CreateInstance(jsonTypeInfo.Type, true);
            //}
        }

        return jsonTypeInfo;
    }
}