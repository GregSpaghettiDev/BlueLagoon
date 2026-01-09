using BlueLagoon.Shared.DevTools.Installers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace BlueLagoon.Shared.Infrastructure.DI.WebApplicationExtensions;

internal sealed class Swagger : IMiddlewaresInstaller
{
    public int InstallOrder => 2;

    public void Install(WebApplication application)
    {
        if (application.Environment.IsDevelopment())
        {
            application.UseSwagger();
            application.UseSwaggerUI();
        }
    }
}