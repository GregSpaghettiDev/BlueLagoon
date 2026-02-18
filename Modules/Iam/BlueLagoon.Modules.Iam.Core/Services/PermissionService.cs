using AutoMapper;
using BlueLagoon.Modules.Iam.Core.DAL;
using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Modules.Iam.Core.Exceptions;
using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Modules.Iam.Core.ValueObjects;
using BlueLagoon.Shared.DevTools.Linq;
using BlueLagoon.Shared.DevTools.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlueLagoon.Modules.Iam.Core.Services;

internal sealed class PermissionService(IamDbContext dbContext, IMapper mapper) : IPermissionService
{
    public async Task CreatePermissionAsync(string name, Guid moduleId, string description)
    {
        var moduleName = await dbContext.Module.AsNoTracking()
                                               .Where(x => x.Id == moduleId)
                                               .Select(x => x.Name)
                                               .SingleOrDefaultAsync();

        if (string.IsNullOrWhiteSpace(moduleName))
            throw new ModuleNotFoundException(moduleId);

        var fullPermissionName = new PermissionName(name, moduleName);

        var permission = Permission.Create(fullPermissionName, description);
        await dbContext.AddAsync(permission);
        await dbContext.SaveChangesAsync();
    }

    public async Task<PermissionDto> GetPermissionAsync(Guid permissionId)
        => await dbContext.Permission.ReturnSingleOrDefaultAsync<Permission, PermissionDto>(x => x.Id == permissionId, false, mapper.ConfigurationProvider);

    public async Task<PaginatedList<PermissionDto>> GetPermissionsAsync(string searchValue, PaginationParameters paginationParameters)
    {
        var queryable = dbContext.Permission
                                    .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchValue))
            queryable = queryable.Where(x => ((x.FullPermissionName.Name ?? "") + (x.FullPermissionName.ModuleName ?? "") + (x.Description ?? "")).Contains(searchValue));

        return await PaginatedList<PermissionDto>.GetPaginatedPageAsync(queryable, paginationParameters, mapper.ConfigurationProvider);
    }

    public async Task UpdatePermissionAsync(Guid permissionId, bool? isActive, string description)
    {
        var permission = await dbContext.Permission.Where(x => x.Id == permissionId).SingleOrDefaultAsync();

        if (permission is null)
            throw new PermissionNotFoundException(permissionId);

        if (isActive.HasValue)
        {
            if (isActive.HasValue)
            {
                if (isActive.Value)
                    permission.Deactivate();

                else
                    permission.Activate();

                await ChangeActivationStateForAllRelatedRoleClaimsAsync(permission.FullPermissionName.ToString(), isActive.Value);
                await ChangeActivationStateForAllRelatedUserClaimsAsync(permission.FullPermissionName.ToString(), isActive.Value);
            }
        }

        if (!string.IsNullOrWhiteSpace(description))
            permission.ChangeDescription(description);

        await dbContext.SaveChangesAsync();
    }

    private async Task ChangeActivationStateForAllRelatedUserClaimsAsync(string permissionName, bool isActive)
    {
        var userClaims = await dbContext.UserClaims.Where(x => x.ClaimValue == permissionName).ToListAsync();

        foreach(var userClaim in userClaims)
        {
            if (isActive && !userClaim.IsActive) userClaim.Activate();
            if (!isActive && userClaim.IsActive) userClaim.Deactivate();
        }
    }

    private async Task ChangeActivationStateForAllRelatedRoleClaimsAsync(string permissionName, bool isActive)
    {
        var permissions = await dbContext.RoleClaims.Where(x => x.ClaimValue == permissionName).ToListAsync();

        foreach (var permission in permissions)
        {
            if (isActive && !permission.IsActive) permission.Activate();
            if (!isActive && permission.IsActive) permission.Deactivate();
        }
    }

    public async Task DeletePermissionAsync(Guid permissionId)
    {
        var permission = await dbContext.Permission.Where(x => x.Id == permissionId).SingleOrDefaultAsync();

        if (permission is null)
            throw new PermissionNotFoundException(permissionId);

        var userClaims = await dbContext.UserClaims.Where(x => x.ClaimValue == permission.FullPermissionName.ToString()).ToListAsync();
        dbContext.RemoveRange(userClaims);

        var roleClaims = await dbContext.RoleClaims.Where(x => x.ClaimValue == permission.FullPermissionName.ToString()).ToListAsync();
        dbContext.RemoveRange(roleClaims);

        await dbContext.SaveChangesAsync();
    }
}