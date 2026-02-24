using AutoMapper;
using BlueLagoon.Modules.Iam.Core.DAL;
using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Modules.Iam.Core.Exceptions;
using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Http;
using BlueLagoon.Shared.DevTools.Linq;
using BlueLagoon.Shared.DevTools.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi;
using OpenIddict.Abstractions;

namespace BlueLagoon.Modules.Iam.Core.Services;

internal sealed class ModuleService(IamDbContext dbContext,
                                    IMapper mapper,
                                    IHttpClientFactory httpClientFactory,
                                    IOpenIddictScopeManager scopeManager,
                                    IHttpContextAccessor httpContextAccessor,
                                    ILogger<ModuleService> logger) 
    : IModuleService
{
    public Task<PaginatedList<ModuleDto>> GetModulesAsync(string searchValue, PaginationParameters paginationParameters = null)
    {
        var queryable = dbContext.Module
                                    .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchValue))
            queryable = queryable.Where(x => x.Name.Contains(searchValue));

        return PaginatedList<ModuleDto>.GetPaginatedPageAsync(queryable, paginationParameters, mapper.ConfigurationProvider);
    }

    public async Task<IEnumerable<EndpointDefinitionDto>> GetEndpointDefinitionsFromOpenApi(string uri, string moduleCode)
    {
        var client = httpClientFactory.CreateClient();

        using var response = await client.GetAsync(uri);
        response.EnsureSuccessStatusCode();

        var openApiContent = await response.Content.ReadAsStringAsync();

        var result = OpenApiDocument.Parse(openApiContent);

        if (result.Diagnostic.Errors.Any())
        {
            logger.LogError("Dokumentacja Open Api zawiera błędy: {errors}", string.Join(", ", result.Diagnostic.Errors.Select(x => x.Message)));
            throw new InvalidOperationException("Dokumentacja Open Api zawiera błędy: " + string.Join(", ", result.Diagnostic.Errors.Select(x => x.Message)));
        }
            
        var discoveredEndpoints =
            result.Document.Paths
                            .SelectMany(p => p.Value.Operations.Select(o =>
                            new EndpointDefinitionDto
                            {
                                Path = p.Key,
                                HttpMethod = o.Key.ToString().ToUpper(),
                                OperationId = o.Value.OperationId,
                                Summary = o.Value.Summary,
                                Tags = o.Value.Tags?.Select(t => t.Name).ToHashSet()
                            }))
                            .Where(e => e.Tags != null && e.Tags.Contains(moduleCode))
                            .ToList();

        var registeredEndpoints = await dbContext.RegisteredEndpoint
                                                        .AsNoTracking()
                                                        .Where(x => x.ModuleName == moduleCode)
                                                        .Select(x => x.Path)
                                                        .ToHashSetAsync();

        discoveredEndpoints.RemoveAll(x => registeredEndpoints.Contains(x.Path));

        logger.LogInformation(string.Join(Environment.NewLine, discoveredEndpoints.Select(x => $"{x.ModuleCode} {x.HttpMethod} {x.Path} {x.OperationId}")));

        return discoveredEndpoints;
    }

    public async Task AddModuleAsync(string name, string description, string openApiUri, string baseUrl)
    {
        if (await dbContext.Module.AnyAsync(x => x.Name == name && x.IsActive))
            throw new ModuleAlreadyExistsException(name);

        var module =
            await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = name,
                DisplayName = description,
                Resources = { Dictionaries.Scope.Resource }
            });

        ((Module)module).SetBaseUrl(baseUrl);
        ((Module)module).SetOpenApiPath(openApiUri);

        httpContextAccessor.HttpContext.AddCreatedResourceId(((Module)module).Id);

        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateModuleAsync(Guid moduleId, string name, string baseUrl, string openApiPath, bool? isActive)
    {
        var module = await dbContext.Module.ReturnSingleOrDefaultAsync(x => x.Id == moduleId, true);
            if (module is null)
                throw new ModuleNotFoundException(moduleId);

        if (!string.IsNullOrWhiteSpace(name))
            module.Name = name;

        if (!string.IsNullOrWhiteSpace(baseUrl))
            module.SetBaseUrl(baseUrl);

        if (!string.IsNullOrWhiteSpace(openApiPath))
            module.SetOpenApiPath(openApiPath);

        if (isActive.HasValue)
        {
            if (isActive.Value)
                module.Activate();

            else
                module.Deactivate();

            await ChangeActivationStateForAllRelatedEndpoinstAsync(module.Name, isActive.Value);
            await ChangeActivationStateForAllRelatedPermissionsAsync(module.Name, isActive.Value);
            await DeleteAssignedPermissionsToRoleAsync(module.Name);
            await DeleteAssignedPermissionsToUserAsync(module.Name);
        }

        await dbContext.SaveChangesAsync();
    }

    private async Task ChangeActivationStateForAllRelatedEndpoinstAsync(string moduleName, bool isActive)
    {
        var endpoints = await dbContext.RegisteredEndpoint.Where(x => x.ModuleName == moduleName).ToListAsync();

        foreach (var endpoint in endpoints)
        {
            if (isActive && !endpoint.IsActive) endpoint.Activate();
            if (!isActive && endpoint.IsActive) endpoint.Deactivate();
        }
    }

    private async Task ChangeActivationStateForAllRelatedPermissionsAsync(string moduleName, bool isActive)
    {
        var permissions = await dbContext.Permission.Where(x => x.FullPermissionName.ModuleName == moduleName).ToListAsync();

        foreach (var permission in permissions)
        {
            if (isActive && !permission.IsActive) permission.Activate();
            if (!isActive && permission.IsActive) permission.Deactivate();
        }
    }

    private async Task DeleteAssignedPermissionsToRoleAsync(string moduleName)
    {
        var roleClaims = await dbContext.RoleClaims.Where(x => x.ModuleName == moduleName).ToListAsync();
        dbContext.RemoveRange(roleClaims);
    }

    private async Task DeleteAssignedPermissionsToUserAsync(string moduleName)
    {
        var userClaims = await dbContext.UserClaims.Where(x => x.ModuleName == moduleName).ToListAsync();
        dbContext.RemoveRange(userClaims);
    }

    public async Task DeleteModuleAsync(Guid moduleId)
    {
        var module = await dbContext.Module.ReturnSingleOrDefaultAsync(x => x.Id == moduleId, true);

        if (module is not null)
        {
            await DeleteAssignedPermissionsToRoleAsync(module.Name);
            await DeleteAssignedPermissionsToUserAsync(module.Name);
            dbContext.Remove(module);
        }

        await dbContext.SaveChangesAsync();
    }
}