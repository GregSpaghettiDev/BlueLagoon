using BlueLagoon.Modules.Iam.Core.Services.Dto;

namespace BlueLagoon.Modules.Iam.Core.Services.Abstractions;

public interface IAuthorizationService
{
    public Task<AuthorizationResultDto> AuthorizeAsync();
}
