using BlueLagoon.Modules.Iam.Core.Services.Dto;
using Microsoft.AspNetCore.Authentication;

namespace BlueLagoon.Modules.Iam.Core.Services.Abstractions;

public interface IAuthorizationService
{
    Task<AuthorizationResultDto> AuthorizeAsync();

    Task<string> LogoutAsync();

    Task<ExchangeResultDto> ExchangeAuthorizationCodeForTokensOrRefreshSessionAsync();
}
