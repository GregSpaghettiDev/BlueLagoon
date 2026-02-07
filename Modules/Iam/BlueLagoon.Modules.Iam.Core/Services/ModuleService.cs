using AutoMapper;
using BlueLagoon.Modules.Iam.Core.DAL;
using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Modules.Iam.Core.Exceptions;
using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Base;
using BlueLagoon.Shared.DevTools.Linq;
using BlueLagoon.Shared.DevTools.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using OpenIddict.Abstractions;

namespace BlueLagoon.Modules.Iam.Core.Services;

internal sealed class ModuleService(IamDbContext dbContext,
                                    IMapper mapper,
                                    IHttpClientFactory httpClientFactory,
                                    IOpenIddictScopeManager scopeManager) 
    : IModuleService
{
    public Task<PaginatedList<ModuleDto>> GetModulesAsync(string searchValue, PaginationParameters paginationParameters = null)
    {
        var queryable = dbContext.Module
                                    .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchValue))
            queryable = queryable.Where(x => ((x.Name ?? "") + (x.DisplayName ?? "")).Contains(searchValue));

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
            throw new InvalidOperationException("Dokumentacja Open Api zawiera błędy: " + string.Join(", ", result.Diagnostic.Errors.Select(x => x.Message)));

        var discoveredEndpoints =
            result.Document.Paths
                            .SelectMany(p => p.Value.Operations.Select(o =>
                            new EndpointDefinitionDto
                            {
                                Path = p.Key,
                                HttpMethod = o.Key.ToString().ToUpper(),
                                OperationId = o.Value.OperationId,
                                Summary = o.Value.Summary,
                                Tags = o.Value.Tags?.Select(t => t.Name).ToList()
                            }))
                            .Where(e => e.Tags != null && e.Tags.Contains(moduleCode))
                            .ToList();

        var registeredEndpoints = await dbContext.RegisteredEndpoint
                                                        .AsNoTracking()
                                                        .Where(x => x.ModuleName == moduleCode)
                                                        .Select(x => x.Path)
                                                        .ToListAsync();

        discoveredEndpoints.RemoveAll(x => registeredEndpoints.Contains(x.Path));

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

        await dbContext.SaveChangesAsync();
    }

    public async Task SetOpenApiPathAsync(BaseId moduleId, string path)
    {
        var pathVo = new ValueObjects.Path(path);

        var module = await dbContext.Module.ReturnSingleOrDefaultAsync(x => x.Id == moduleId.Id && x.IsActive, true)
            ?? throw new ModuleNotFoundException(moduleId);

        module.SetOpenApiPath(pathVo);
        await dbContext.SaveChangesAsync();
    }

    public async Task SetOpenApiUrlAsync(BaseId moduleId, string url)
    {
        var module = await dbContext.Module.ReturnSingleOrDefaultAsync(x => x.Id == moduleId.Id && x.IsActive, true)
            ?? throw new ModuleNotFoundException(moduleId);

        if (string.IsNullOrWhiteSpace(url))
            throw new InvalidOpenApiUrlException();

        module.SetBaseUrl(url);
        await dbContext.SaveChangesAsync();
    }

    public async Task SetOpenApiUrlAndOrPathAsync(BaseId moduleId, string url = null, string path = null)
    {
        var module = await dbContext.Module.ReturnSingleOrDefaultAsync(x => x.Id == moduleId.Id && x.IsActive, true)
            ?? throw new ModuleNotFoundException(moduleId);

        if (!string.IsNullOrWhiteSpace(url))
            module.SetBaseUrl(url);

        if (!string.IsNullOrWhiteSpace(path))
        {
            var pathVo = new ValueObjects.Path(path);
            module.SetOpenApiPath(pathVo);
        }

        if (dbContext.ChangeTracker.HasChanges())
            await dbContext.SaveChangesAsync();
    }

    public async Task DeactivateModuleAsync(BaseId moduleId)
    {
        var module = await dbContext.Module.ReturnSingleOrDefaultAsync(x => x.Id == moduleId.Id && x.IsActive, true)
            ?? throw new ModuleNotFoundException(moduleId);

        module.Deactivate();
        await dbContext.SaveChangesAsync();
    }

    public async Task ActivateModuleAsync(BaseId moduleId)
    {
        var module = await dbContext.Module.ReturnSingleOrDefaultAsync(x => x.Id == moduleId.Id && !x.IsActive, true)
            ?? throw new ModuleNotFoundException(moduleId);

        module.Activate();
        await dbContext.SaveChangesAsync();
    }

    public async Task ChangeNameAsync(BaseId moduleId, string newName)
    {
        var module = await dbContext.Module.ReturnSingleOrDefaultAsync(x => x.Id == moduleId.Id, true)
            ?? throw new ModuleNotFoundException(moduleId);

        module.Name = newName;
        await dbContext.SaveChangesAsync();
    }
}