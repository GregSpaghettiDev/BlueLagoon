using AutoMapper;
using BlueLagoon.Modules.Iam.Core.DAL;
using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Modules.Iam.Core.Exceptions;
using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Linq;
using BlueLagoon.Shared.DevTools.Pagination;
using Microsoft.EntityFrameworkCore;

namespace BlueLagoon.Modules.Iam.Core.Services;

internal sealed class EndpointService(IamDbContext dbContext, IMapper mapper) : IEndpointService
{
    public async Task<PaginatedList<RegisteredEndpointDto>> GetEndpointsAsync(string moduleName, string httpMethod, string path, bool withoutPermissionMappings, PaginationParameters paginationParameters)
    {
        var queryable = dbContext.RegisteredEndpoint.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(moduleName))
            queryable = queryable.Where(x => x.ModuleName.Contains(moduleName));

        if (!string.IsNullOrWhiteSpace(httpMethod))
            queryable = queryable.Where(x => x.HttpMethod.Method == httpMethod);

        if (!string.IsNullOrWhiteSpace(path))
            queryable = queryable.Where(x => x.Path.Contains(path));

        if (withoutPermissionMappings)
            queryable = queryable.Where(x => x.RegisteredEndpointPermissions.Count == 0);

        return await PaginatedList<RegisteredEndpointDto>.GetPaginatedPageAsync(queryable, paginationParameters, mapper.ConfigurationProvider);
    }

    public async Task<RegisteredEndpointDto> GetEndpointAsync(Guid endpointId)
        => await dbContext.RegisteredEndpoint.ReturnSingleOrDefaultAsync<RegisteredEndpoint, RegisteredEndpointDto>(x => x.Id == endpointId, false, mapper.ConfigurationProvider);

    public async Task<EndpointMappingsDto> GetEndpointSecurityMappingsAsync(Guid endpointId, IList<Guid> permissionIds, bool filterAvailable, bool filterAssigned, PaginationParameters paginationParameters)
    {
        var targetModuleName = await dbContext.RegisteredEndpoint
                                                        .Where(x => x.Id == endpointId)
                                                        .Select(x => x.ModuleName.Value)
                                                        .SingleOrDefaultAsync();

        if (targetModuleName is null)
            throw new EndpointNotFoundException(endpointId);

        var assignedQueryable = dbContext.RegisteredEndpointPermission.AsNoTracking().Where(x => x.RegisteredEndpointId == endpointId);
        var availableQueryable = dbContext.Permission.AsNoTracking().Where(x => x.FullPermissionName.ModuleName == targetModuleName);
        
        var filterPattern = (filterAssigned, filterAvailable, permissionIds.Count > 0);
        (assignedQueryable, availableQueryable) = filterPattern switch
        {
            (true, false, true) => (assignedQueryable.Where(x => permissionIds.Contains(x.PermissionId)), availableQueryable),
            (false, true, true) => (assignedQueryable, availableQueryable.Where(x => permissionIds.Contains(x.Id))),
            (true, true, true) => (assignedQueryable.Where(x => permissionIds.Contains(x.PermissionId)), availableQueryable.Where(x => permissionIds.Contains(x.Id))),
            _ => (assignedQueryable, availableQueryable)
        };

        availableQueryable = availableQueryable.Where(x => !dbContext.RegisteredEndpointPermission.Any(rep => rep.RegisteredEndpointId == endpointId && rep.PermissionId == x.Id));

        var assignedRequirements = await PaginatedList<EndpointRequirementDto>.GetPaginatedPageAsync(assignedQueryable, paginationParameters, mapper.ConfigurationProvider);
        var availableRequirements = await PaginatedList<EndpointRequirementDto>.GetPaginatedPageAsync(availableQueryable, paginationParameters, mapper.ConfigurationProvider);

        return new EndpointMappingsDto
        {
            AssignedRequirementsDto = assignedRequirements,
            AvailableRequirementsDto = availableRequirements
        };
    }

    public async Task UpdateEndpointRestrictionsAsync(Guid endpointId, IList<Guid> requirementIds)
    {
        if (requirementIds is null) 
            return;

        var endpointExists = await dbContext.RegisteredEndpoint.AnyAsync(x => x.Id == endpointId);
        if (!endpointExists)
            throw new EndpointNotFoundException(endpointId);

        var currentRequirementIds = await dbContext.RegisteredEndpointPermission.Where(x => x.RegisteredEndpointId == endpointId)
                                                                                .Select(x => x.PermissionId)
                                                                                .ToHashSetAsync();

        var requirementIdsToAdd = requirementIds.Where(x => !currentRequirementIds.Contains(x));
        var requirementsToAdd = requirementIdsToAdd.Select(x => RegisteredEndpointPermission.Create(x, endpointId));

        await dbContext.RegisteredEndpointPermission.Where(x => x.RegisteredEndpointId == endpointId && !requirementIds.Contains(x.PermissionId))
                                                    .ExecuteDeleteAsync();
        dbContext.RegisteredEndpointPermission.AddRange(requirementsToAdd);

        await dbContext.SaveChangesAsync();
    }
}