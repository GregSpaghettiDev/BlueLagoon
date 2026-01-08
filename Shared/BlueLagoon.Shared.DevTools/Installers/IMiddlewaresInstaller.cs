using Microsoft.AspNetCore.Builder;

namespace BlueLagoon.Shared.DevTools.Installers;

public interface IMiddlewaresInstaller
{
    void Install(WebApplication application);
}