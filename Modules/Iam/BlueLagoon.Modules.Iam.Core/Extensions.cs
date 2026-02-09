using BlueLagoon.Modules.Iam.Core.DI.ServiceExtensions;
using BlueLagoon.Modules.Iam.Core.Dictionaries;
using BlueLagoon.Shared.DevTools.Installers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BlueLagoon.Modules.Iam.Api")]
namespace BlueLagoon.Modules.Iam.Core;

internal static class Extensions
{
    public static void InstallIamServices(this IServiceCollection services, IConfiguration configuration)
    {
        foreach (var installer in InstallersFetcher.GetInstallers<Oauth2Oidc, IServicesInstaller>())
            installer.Intstall(services, configuration);
    }

    public static void ConfigureIamPolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(AuthorizationPolicies.IamScopeRequirement, p => p.RequireClaim(OpenIddictConstants.Claims.Scope, "iam"));
        options.AddPolicy(AuthorizationPolicies.ReadModules, p => p.RequireAssertion(context => context.User.IsInRole(ValueObjects.Role.IamAdminRoleName) || context.User.HasClaim("permission", "iam.read-modules")));
        options.AddPolicy(AuthorizationPolicies.AddModule, p => p.RequireAssertion(context => context.User.IsInRole(ValueObjects.Role.IamAdminRoleName) || context.User.HasClaim("permission", "iam.add-module")));
        options.AddPolicy(AuthorizationPolicies.UpdateModule, p => p.RequireAssertion(context => context.User.IsInRole(ValueObjects.Role.IamAdminRoleName) || context.User.HasClaim("permission", "iam.update-module")));
        options.AddPolicy(AuthorizationPolicies.ReadModules, p => p.RequireAssertion(context => context.User.IsInRole(ValueObjects.Role.IamAdminRoleName) || context.User.HasClaim("permission", "iam.read-roles")));
    }
}