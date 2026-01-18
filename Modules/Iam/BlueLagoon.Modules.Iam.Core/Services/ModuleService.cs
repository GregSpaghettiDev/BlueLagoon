using AutoMapper;
using BlueLagoon.Modules.Iam.Core.DAL;
using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Sprache;

namespace BlueLagoon.Modules.Iam.Core.Services;

internal sealed class ModuleService(IamDbContext dbContext, IMapper mapper, IHttpClientFactory httpClientFactory) : IModuleService
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

        var openApiContent =  await response.Content.ReadAsStringAsync();

        var result = OpenApiDocument.Parse(openApiContent);
        
        if (result.Diagnostic.Errors.Any())
            throw new InvalidOperationException("Dokumentacja Open Api zawiera błędy: " + string.Join(", ", result.Diagnostic.Errors.Select(x => x.Message)));

        var endpoints =
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

        return endpoints;
    }
}