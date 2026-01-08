using BlueLagoon.Shared.DevTools.Installers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace BlueLagoon.Shared.Infrastructure.DI.WebApplicationExtensions;

internal sealed class Cors : IMiddlewaresInstaller
{
    public void Install(WebApplication application)
    {
        if (application.Environment.IsDevelopment())
        {
            application.UseCors(options =>
            {
                options
                    .AllowAnyOrigin()
                    .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                    .WithExposedHeaders("Content-Disposition")
                    .AllowAnyHeader();
            });
        }
    }
}