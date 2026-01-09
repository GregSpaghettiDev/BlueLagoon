using BlueLagoon.Shared.DevTools.Installers;
using Microsoft.AspNetCore.Builder;

namespace BlueLagoon.Shared.Infrastructure.DI.WebApplicationExtensions;

internal sealed class Controllers : IMiddlewaresInstaller
{
    public int InstallOrder => 3;

    public void Install(WebApplication application)
    {
        application.MapControllers();
    }
}