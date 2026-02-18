using AutoMapper;
using BlueLagoon.Modules.Iam.Core.DAL;
using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Modules.Iam.Core.Dictionaries;
using BlueLagoon.Modules.Iam.Core.Exceptions;
using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Http;
using BlueLagoon.Shared.DevTools.Linq;
using BlueLagoon.Shared.DevTools.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlueLagoon.Modules.Iam.Core.Services;

internal sealed class RoleService(IamDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, RoleManager<Role> roleManager) : IRoleService
{
    public async Task<PaginatedList<RoleDto>> GetRolesAsync(string searchValue, PaginationParameters paginationParameters)
    {
        var queryable = dbContext.Roles
                                    .AsNoTracking();

        var normalizedSearchValue = searchValue.ToUpperInvariant(); 

        if (!string.IsNullOrWhiteSpace(searchValue))
            queryable = queryable = queryable.Where(x => x.NormalizedName.Contains(normalizedSearchValue));

        return await PaginatedList<RoleDto>.GetPaginatedPageAsync(queryable, paginationParameters, mapper.ConfigurationProvider);
    }

    public async Task<RoleWithPermissionsDto> GetRoleAsync(Guid roleId)
    {
        var role = await dbContext.Roles.ReturnSingleOrDefaultAsync<DAL.Entities.Role, RoleWithPermissionsDto>(x => x.Id == roleId, false, mapper.ConfigurationProvider);

        var claimIds = role.AssignedPermissions.Where(x => x.ClaimId != null).Select(x => x.ClaimId);
        role.AvailablePermissions = await dbContext.Permission.ReturnListAsync<Permission, ClaimDto>(x => x.FullPermissionName.ModuleName == Scope.Iam.Name && !claimIds.Contains(x.Id), false, mapper.ConfigurationProvider);

        return role;
    }

    async Task UpdateOrCreateRolePermissionsAsync(Guid roleId, IList<Guid> permissionIds)
    {
        var currentRoleClaims = await dbContext.RoleClaims
                                                    .Where(x => x.RoleId == roleId)
                                                    .ToListAsync();

        var desiredRoleClaimIds = permissionIds is null 
                                        ? currentRoleClaims.Select(x => x.ClaimId ?? Guid.Empty).Where(x => x != Guid.Empty).ToHashSet()
                                        : permissionIds.ToHashSet();
        
      
        var roleClaimsToRemove = currentRoleClaims.Where(x => !desiredRoleClaimIds.Contains(x.ClaimId ?? Guid.Empty)).Where(x => x.ClaimId != null).ToArray();

        var currentRoleClaimIds = currentRoleClaims.Select(x => x.ClaimId ?? Guid.Empty).Where(x => x != Guid.Empty).ToHashSet();
        var roleClaimIdsToAdd = desiredRoleClaimIds.Where(x => !currentRoleClaimIds.Contains(x)).ToArray();
        
        if (roleClaimsToRemove.Length > 0)
            dbContext.RemoveRange(roleClaimsToRemove);

        if (roleClaimIdsToAdd.Length > 0)
        {
            var permissionsToAdd = await dbContext.Permission.AsNoTracking().Where(x => roleClaimIdsToAdd.Contains(x.Id)).Select(x => new RoleClaim
            {
                RoleId = roleId,
                ClaimType = "permission",
                ClaimValue = x.FullPermissionName.FullPermissionName,
                ClaimDescription = x.Description,
                ClaimId = x.Id,
                ModuleName = x.FullPermissionName.ModuleName
            }).ToListAsync();
            
            dbContext.RoleClaims.AddRange(permissionsToAdd);
        }
    }

    public async Task CreateRoleAsync(ValueObjects.Role role, IList<Guid> permissionIds)
    {
        if (await dbContext.Roles.AnyAsync(x => x.NormalizedName == role.NormalizedName || x.Id == role.Id))
            throw new RoleAlreadyExistsException(role.Id.ToString(), role.Name, role.DisplayRoleName);

        using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            await roleManager.CreateAsync(Role.Create(role));

            httpContextAccessor.HttpContext.AddCreatedResourceId(role.Id);

            await UpdateOrCreateRolePermissionsAsync(role.Id, permissionIds);

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateRoleAsync(Guid roleId, IList<Guid> permissionIds, bool? isActive)
    {
        var role = await dbContext.Roles.SingleOrDefaultAsync(x => x.Id == roleId && x.Id != ValueObjects.Role.IamAdmin.Id);
        if (role is null)
            throw new RoleNotFoundException(roleId);

        if (isActive.HasValue)
        {
            if (isActive.Value)
                role.Activate();

            else
                role.Deactivate();
            
            await ChangeActivationStateForAllAssignedPermissionsAsync(role.Id, isActive.Value);
        }

        if (permissionIds?.Any() ?? false && isActive != false)
            await UpdateOrCreateRolePermissionsAsync(roleId, permissionIds);

        if (dbContext.ChangeTracker.HasChanges())
            await dbContext.SaveChangesAsync();

    }

    private async Task ChangeActivationStateForAllAssignedPermissionsAsync(Guid roleId, bool isActive)
    {
        var permissions = await dbContext.RoleClaims.Where(x => x.RoleId == roleId).ToListAsync();

        foreach (var permission in permissions)
        {
            if (isActive && !permission.IsActive) permission.Activate();
            if (!isActive && permission.IsActive) permission.Deactivate();
        }
    }

    public async Task DeleteRoleAsync(Guid roleId)
    {
        var role = await dbContext.Roles
                                    .Include(x => x.RoleClaims)
                                    .Include(x => x.UserRoles)
                                    .Where(x => x.Id == roleId && x.Id != ValueObjects.Role.IamAdmin.Id)
                                    .SingleOrDefaultAsync();

        if (role is not null)
        {
            if (role.RoleClaims.Count > 0) dbContext.RemoveRange(role.RoleClaims);
            if (role.UserRoles.Count > 0) dbContext.RemoveRange(role.UserRoles);

            await roleManager.DeleteAsync(role);

            await dbContext.SaveChangesAsync();
        }
    }
}