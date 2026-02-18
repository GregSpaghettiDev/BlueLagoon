using BlueLagoon.Modules.Iam.Api.Controllers.Role.Requests;
using BlueLagoon.Modules.Iam.Api.Controllers.User.Requests;
using BlueLagoon.Modules.Iam.Core.Dictionaries;
using BlueLagoon.Modules.Iam.Core.Services;
using BlueLagoon.Modules.Iam.Core.Services.Abstractions;
using BlueLagoon.Modules.Iam.Core.Services.Dto;
using BlueLagoon.Shared.DevTools.Api;
using BlueLagoon.Shared.DevTools.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace BlueLagoon.Modules.Iam.Api.Controllers.Identity;

[ApiController]
[Tags(IamModule.BasePath)]
[Route(IamModule.BasePath + "/users")]
[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Policy = AuthorizationPolicies.IamScopeRequirement)]

internal sealed class UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
    : BaseController(httpContextAccessor)
{
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.ReadUsers)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PaginatedList<UserDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [SwaggerOperation(OperationId = nameof(GetUsersAsync), 
        Summary = "Pobranie listy użytkowników", 
        Description = "Zwraca listę użytkowników z możliwością filtrowania po dostępnych parametrach.")]
    public async Task<IActionResult> GetUsersAsync([FromQuery] UserListRequest request)
    {
        var result = await userService.GetUsersAsync(request.FirstName,
                                                     request.LastName,
                                                     request.Email,
                                                     request.PaginationParameters,
                                                     request.RoleIds,
                                                     request.PermissionNames);

        return ContentResult(result, HttpStatusCode.OK);
    }

    [HttpGet("{userId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.ReadUsers)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(BaseUserDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [SwaggerOperation(OperationId = nameof(GetUserAsync), 
        Summary = "Pobranie informacji o użytkowniku", 
        Description = "Zwraca inforacje dotyczące konkretnego użytkownika wraz z bezpośrednio przypisanymi uprawnieniami")]
    public async Task<IActionResult> GetUserAsync([FromRoute] Guid userId)
    {
        var result = await userService.GetUserAsync(userId);

        return ContentResult(result, HttpStatusCode.OK);
    }

    [HttpGet("{userId:guid}/roles")]
    [Authorize(Policy = AuthorizationPolicies.ReadUsers)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PaginatedList<UserRoleDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [SwaggerOperation(OperationId = nameof(GetUserRolesAsync),
        Summary = "Pobranie informacji o użytkowniku",
        Description = "Zwraca inforacje dotyczące konkretnego użytkownika wraz z bezpośrednio przypisanymi uprawnieniami")]
    public async Task<IActionResult> GetUserRolesAsync([FromRoute] Guid userId, [FromQuery] UserRolesListRequest request)
    {
        var result = await userService.GetUserRolesAsync(userId,
                                                         request.PaginationParameters,
                                                         request.RoleIds,
                                                         request.PermissionNames,
                                                         request.ModuleNames);

        return ContentResult(result, HttpStatusCode.OK);
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.AddOrDeleteUser)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [SwaggerOperation(OperationId = nameof(CreateUserAsync),
        Summary = "Dodanie nowego użytkownika z ewentualnie przypisanymi uprawnieniami",
        Description = "Dodaje nowego użytkownika z możliwością jednoczesnego przypisania uprawnień.")]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
    {
        await userService.CreateUserAsync(request.FirstName,
                                          request.LastName,
                                          request.PhoneNumber,
                                          request.Email,
                                          request.Password,
                                          request.RepeatedPassword,
                                          request.RoleIds,
                                          request.PermissionIds);

        return NoContentResult();
    }

    [HttpPatch("{userId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.AddOrDeleteUser)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)]
    [SwaggerOperation(OperationId = nameof(UpdateUserRequest),
        Summary = "Zmienia wartości poszczególnych pól użytkownika takich jak stan aktywny/nieaktywny, imię, nazwisko, email, przypisane uprawnienia.",
        Description = "Do zmiany poszczególnych wartości należy w żądaniu zdefiniować odpowiednie pola i ich wartości (IsActive, PermissionIds, FirstName, LastName, Email)." +
        "Nie można jednocześnie przypisać uprawnień i dezaktywować użytkownika.")]
    public async Task<IActionResult> UpdateRoleAsync([FromRoute] Guid userId, [FromBody] UpdateUserRequest request)
    {
        await userService.UpdateUserAsync(userId, request.IsActive, request.FirstName, request.LastName, request.Email, request.PermissionIds, request.RoleIds);

        return NoContentResult();
    }
}