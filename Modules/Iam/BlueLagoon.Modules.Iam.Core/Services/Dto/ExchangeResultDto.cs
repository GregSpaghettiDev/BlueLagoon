using Microsoft.AspNetCore.Authentication;

namespace BlueLagoon.Modules.Iam.Core.Services.Dto;

public class ExchangeResultDto
{
    public AuthenticateResult ExchangeResult { get; set; }

    public bool IsRefreshTokenGrant { get; set; }
}
