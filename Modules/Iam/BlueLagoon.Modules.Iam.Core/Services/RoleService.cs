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
using Microsoft.EntityFrameworkCore;

namespace BlueLagoon.Modules.Iam.Core.Services;

internal sealed class RoleService(IamDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor) : IRoleService
{
    public async Task<PaginatedList<RoleDto>> GetRolesAsync(string searchValue, PaginationParameters paginationParameters)
    {
        var queryable = dbContext.Roles
                                    .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchValue))
            queryable = queryable = queryable.Where(x => ((x.Name ?? "") + (x.DisplayName ?? "")).Contains(searchValue));

        return await PaginatedList<RoleDto>.GetPaginatedPageAsync(queryable, paginationParameters, mapper.ConfigurationProvider);
    }

    public async Task<RoleWithPermissionsDto> GetRoleAsync(Guid roleId)
    {
        var role = await dbContext.Roles.ReturnSingleOrDefaultAsync<Role, RoleWithPermissionsDto>(x => x.Id == roleId, false, mapper.ConfigurationProvider);

        var claimIds = role.AssignedPermissions.Where(x => x.ClaimId != null).Select(x => x.ClaimId);
        role.AvailablePermissions = await dbContext.Permission.ReturnListAsync<Permission, ClaimDto>(x => x.FullPermissionName.ModuleName == Scope.Iam.Name && !claimIds.Contains(x.Id), false, mapper.ConfigurationProvider);

        return role;
    }

    async Task UpdateRolePermissionsAsync(Guid roleId, IList<Guid> permissionIds)
    {
        var roleClaims = await dbContext.RoleClaims
                            .Where(x => x.RoleId == roleId).ToListAsync();

        if (permissionIds.Count > 0)
        {
            var roleClaimsToRemove = roleClaims.Where(x => !permissionIds.Contains(x.ClaimId ?? Guid.Empty)).ToList();
            var permissionIdsToAdd = permissionIds.Except(roleClaimsToRemove?.Select(x => x.ClaimId ?? Guid.Empty) ?? []).Where(x => x != Guid.Empty);
            var roleClaimsToAdd = await dbContext.Permission.ReturnListAsync<Permission, RoleClaim>(x => permissionIdsToAdd.Contains(x.Id), false, mapper.ConfigurationProvider);

            foreach (var permission in roleClaimsToAdd)
                dbContext.AddRange(roleClaimsToAdd);
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task CreateRoleAsync(ValueObjects.Role role, IList<Guid> permissionIds)
    {
        if (await dbContext.Roles.AnyAsync(x => x.Name == role.Name || x.DisplayName == role.DisplayRoleName || x.Id == role.Id))
            throw new RoleAlreadyExistsException(role.Id.ToString(), role.Name, role.DisplayRoleName);

        await dbContext.AddAsync(Role.Create(role));

        httpContextAccessor.HttpContext.AddCreatedResourceId(role.Id);

        await UpdateRolePermissionsAsync(role.Id, permissionIds);
    }

    public async Task UpdateRoleAsync(Guid roleId, IList<Guid> permissionIds, bool? isActive)
    {
        var role = await dbContext.Roles.SingleOrDefaultAsync(x => x.Id == roleId);
        if (role is null)
            throw new RoleNotFoundException(roleId);

        if (isActive.HasValue)
            if (isActive.Value)
                role.Deactivate();
            else
                role.Activate();

        if (permissionIds?.Any() ?? false)
            await UpdateRolePermissionsAsync(roleId, permissionIds);

    }
}