using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlueLagoon.Modules.Iam.Core.DAL;
using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using BlueLagoon.Modules.Iam.Core.Exceptions;
using BlueLagoon.Modules.Iam.Core.Exceptions.Policies.Abstractions;
using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Modules.Iam.Core.ValueObjects;
using BlueLagoon.Shared.DevTools.Linq;
using BlueLagoon.Shared.DevTools.Pagination;
using BlueLagoon.Shared.Infrastructure.Exceptions.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlueLagoon.Modules.Iam.Core.Services;

internal sealed class UserService(UserManager<User> userManager,
                                  IMapper mapper,
                                  IamDbContext dbContext,
                                  IEnumerable<IIdentityErrorPolicy> identityErrorPolicies) 
    : IUserService
{
    public async Task<PaginatedList<UserDto>> GetUsersAsync(string firstName, string lastName, string email, PaginationParameters paginationParameters, IEnumerable<Guid> roleIds = null, IEnumerable<string> permissionNames = null)
    {
        var queryable = dbContext.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(firstName))
            queryable = queryable.Where(x => x.FirstName.Contains(firstName));

        if (!string.IsNullOrWhiteSpace(lastName))
            queryable = queryable.Where(x => x.LastName.Contains(lastName));

        if (!string.IsNullOrWhiteSpace(email))
            queryable = queryable.Where(x => x.Email.Contains(email));

        if (roleIds?.Any() ?? false)
            queryable = queryable.Where(x => x.UserRoles.Any(ur => roleIds.Contains(ur.RoleId)));

        if (permissionNames?.Any() ?? false)
            queryable = queryable.Where(x => x.UserClaims.Any(uc => permissionNames.Contains(uc.ClaimValue)));

        return await PaginatedList<UserDto>.GetPaginatedPageAsync(queryable, paginationParameters, mapper.ConfigurationProvider);
    }

    public async Task<BaseUserDto> GetUserAsync(Guid id)
        => await dbContext.Users.ReturnSingleOrDefaultAsync<User, BaseUserDto>(x => x.Id == id, false, mapper.ConfigurationProvider);

    public async Task<PaginatedList<UserRoleDto>> GetUserRolesAsync(Guid userId, PaginationParameters paginationParameters, IEnumerable<Guid> roleIds = null, IEnumerable<string> permissionNames = null, IEnumerable<string> moduleNames = null)
    {
        var queryable = dbContext.UserRoles.AsNoTracking().Where(x => x.UserId == userId);

        if (roleIds?.Any() ?? false)
            queryable = queryable.Where(x => roleIds.Contains(x.RoleId));

        if (permissionNames?.Any() ?? false)
            queryable = queryable.Where(x => x.Role.RoleClaims.Any(x => permissionNames.Contains(x.ClaimValue)));

        if (moduleNames?.Any() ?? false)
            queryable = queryable.Where(x => x.Role.RoleClaims.Any(x => moduleNames.Contains(x.ModuleName)));

        return await PaginatedList<UserRoleDto>.GetPaginatedPageAsync(queryable, paginationParameters, mapper.ConfigurationProvider);
    }

    public async Task<PaginatedList<UserPermissionDto>> GetUserPermissionsAsync(Guid userId, PaginationParameters paginationParameters, IEnumerable<Guid> roleIds = null, IEnumerable<string> permissionNames = null, IEnumerable<string> moduleNames = null)
    {
        var userPermissionsQueryable = dbContext.UserClaims
                                                       .AsNoTracking()
                                                       .Where(x => x.UserId == userId)
                                                       .ProjectTo<UserPermissionDto>(mapper.ConfigurationProvider);

        var rolePermissionsQuery = dbContext.UserRoles
                                                .Where(ur => ur.UserId == userId && (roleIds == null || roleIds.Contains(ur.RoleId)))
                                                .SelectMany(ur => ur.Role.RoleClaims)
                                                .ProjectTo<UserPermissionDto>(mapper.ConfigurationProvider);

        var finalQueryable = userPermissionsQueryable.Concat(rolePermissionsQuery);

        var totalCount = await finalQueryable.CountAsync();
        var result = await finalQueryable
                                .Paginate(paginationParameters, true)
                                .ToListAsync();

        return PaginatedList<UserPermissionDto>.GetPaginatePageFromList(result, totalCount, paginationParameters);
    }

    public async Task<CreatedUserDto> CreateUserAsync(string firstName, string lastName, string phoneNumber, string email, string password, string repeatedPassword, IEnumerable<Guid> roleIds, IEnumerable<Guid> permissionIds)
    {
        UserFirstName userFirstName = new(firstName);
        UserLastName userLastName = new(lastName);
        UserName userName = new(userFirstName, userLastName);
        userName = await GetUserNameAsync(userName);
        var user = User.Create(userFirstName, userLastName, userName);

        if (!string.IsNullOrWhiteSpace(phoneNumber))
            user.PhoneNumber = phoneNumber;

        if (!string.IsNullOrWhiteSpace(email))
            user.Email = email;

        if (password != repeatedPassword)
            throw new PasswordsDoesNotMatchException();

        var identityResult = await userManager.CreateAsync(user, password);

        if (!identityResult.Succeeded)
        {
            var errors = identityResult.Errors.ToList();
            if (errors is [var error])
            {
                var policy = identityErrorPolicies.Single(x => x.CanHandle(error.Code));
                policy.Handle(error);
            }

            List<BaseCoreException> identityExceptions = [];
            foreach (var err in errors)
            {
                var policy = identityErrorPolicies.Where(x => x.CanHandle(err.Code)).Single();
                identityExceptions.Add(policy.Handle(err, true));
            }

            throw new AggregateException("Nieudana próba utworzenia użytkownika.", identityExceptions);
        }

        List<string> rolePermissions = [];
        if (roleIds?.Any() ?? false)
        {
            var roles = await dbContext.Roles.AsNoTracking()
                                             .Where(x => roleIds.Contains(x.Id))
                                             .Select(x => new 
                                             { 
                                                 x.Name,
                                                 Permissions = x.RoleClaims.Select(x => x.ClaimValue)
                                             })
                                             .ToListAsync();
            rolePermissions.AddRange(roles.SelectMany(x => x.Permissions));

            await userManager.AddToRolesAsync(user, roles.Select(x => x.Name));
        }

        HashSet<string> permissionsExistingInRoles = [];
        List<Permission> permissions = [];
        List<string> permissionsToAdd = [];
        if (permissionIds?.Any() ?? false)
        {
            permissions = await dbContext.Permission.AsNoTracking()
                                                        .Where(x => permissionIds.Contains(x.Id))
                                                        .ToListAsync();

            var permissionNames = permissions.Select(x => x.FullPermissionName.FullPermissionName).ToList();
            var rolePermissionSet = rolePermissions.ToHashSet<string>();
            permissionsExistingInRoles = permissionNames.Where(x => rolePermissionSet.Contains(x)).ToHashSet<string>();
            permissionsToAdd = permissionNames.Where(x => !permissionsExistingInRoles.Contains(x)).ToList();

            await userManager.AddClaimsAsync(user, permissionsToAdd.Select(x => new Claim("permission", x)));
        }

        return new CreatedUserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            PermissionsOccuredInRoles = permissions.Where(x => permissionsExistingInRoles.Contains(x.FullPermissionName.FullPermissionName)).Select(x => new PermissionOccuredInRoleDto
            {
                Id = x.Id,
                Description = x.Description,
                ModuleName = x.FullPermissionName.ModuleName,
                Name = x.FullPermissionName.FullPermissionName
            })
        };
    }

    private async Task<UserName> GetUserNameAsync(UserName baseUserName)
    {
        var existingUserNames = await dbContext.Users.AsNoTracking()
                                                     .Where(u => u.UserName.StartsWith(baseUserName.Value))
                                                     .Select(u => u.UserName)
                                                     .ToListAsync();

        if (existingUserNames.Count == 0)
            return baseUserName;

        var suffix = 
            existingUserNames.Select(x =>
            {
                var suffixPart = x.Substring(baseUserName.Value.Length);
                return int.TryParse(suffixPart, out int result) ? result : 0;
            }).Max();

        return new(baseUserName.FirstName, $"{baseUserName.LastName}{suffix+1}");
    }
}