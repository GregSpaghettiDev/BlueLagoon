using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace BlueLagoon.Shared.DevTools.Api;

internal class InternalControllerFeatureProvider : ControllerFeatureProvider
{
    private const string ControllerTypeNameSuffix = "Controller";

    protected override bool IsController(TypeInfo typeInfo)
    {
        if (!typeInfo.IsClass)
            return false;

        if (typeInfo.IsAbstract)
            return false;

        if (typeInfo.ContainsGenericParameters)
            return false;

        if (typeInfo.IsDefined(typeof(NonControllerAttribute)))
            return false;

        if (!typeInfo.Name.EndsWith(ControllerTypeNameSuffix, StringComparison.OrdinalIgnoreCase) && !typeInfo.IsDefined(typeof(ControllerAttribute)) && !typeInfo.IsDefined(typeof(ApiControllerAttribute)))
            return false;

        return true;
    }
}