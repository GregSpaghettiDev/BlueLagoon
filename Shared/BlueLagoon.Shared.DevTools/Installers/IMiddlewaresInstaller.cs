using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace BlueLagoon.Shared.DevTools.Installers;

public interface IMiddlewaresInstaller
{
    public int InstallOrder { get; }

    void Install(WebApplication application, IEnumerable<Assembly> loadedAssemblies = null);
}