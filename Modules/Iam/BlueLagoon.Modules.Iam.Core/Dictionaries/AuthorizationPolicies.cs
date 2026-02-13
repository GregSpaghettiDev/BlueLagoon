namespace BlueLagoon.Modules.Iam.Core.Dictionaries;

internal static class AuthorizationPolicies
{
    public const string IamScopeRequirement = nameof(IamScopeRequirement);

    public const string ReadModules = nameof(ReadModules);

    public const string AddOrDeleteModule = nameof(AddOrDeleteModule);

    public const string UpdateModule = nameof(UpdateModule);

    public const string ReadRoles = nameof(ReadRoles);

    public const string UpdateRole = nameof(UpdateRole);

    public const string AddOrDeleteRole = nameof(AddOrDeleteRole);

    public const string ReadPermissions = nameof(ReadPermissions);

    public const string UpdatePermission = nameof(UpdatePermission);

    public const string AddOrDeletePermission = nameof(AddOrDeletePermission);

    public const string ReadUsers = nameof(ReadUsers);

    public const string UpdateUser = nameof(UpdateUser);

    public const string AddOrDeleteUser = nameof(AddOrDeleteUser);
}