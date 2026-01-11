using BlueLagoon.Shared.DevTools.Installers;
using Microsoft.AspNetCore.Builder;

namespace BlueLagoon.Shared.Infrastructure.DI.WebApplicationExtensions;

internal sealed class AuthenticationAndAuthorization : IMiddlewaresInstaller
{
    public int InstallOrder => 2;
    public void Install(WebApplication application)
    {
        application.UseAuthentication();
        application.UseAuthorization();
    }
}
