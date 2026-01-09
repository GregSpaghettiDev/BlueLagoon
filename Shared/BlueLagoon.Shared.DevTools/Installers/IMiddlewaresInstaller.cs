using Microsoft.AspNetCore.Builder;

namespace BlueLagoon.Shared.DevTools.Installers;

public interface IMiddlewaresInstaller
{
    public int InstallOrder { get; }

    void Install(WebApplication application);
}