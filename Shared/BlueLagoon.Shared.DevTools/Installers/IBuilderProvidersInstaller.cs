using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace BlueLagoon.Shared.DevTools.Installers;


public interface IBuilderProvidersInstaller
{
    void Install(WebApplicationBuilder builder, IConfiguration configuration);
}
