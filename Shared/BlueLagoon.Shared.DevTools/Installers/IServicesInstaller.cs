using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlueLagoon.Shared.DevTools.Installers;

public interface IServicesInstaller
{
    void Intstall(IServiceCollection services, IConfiguration configuration);
}