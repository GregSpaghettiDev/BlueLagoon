using System.Security.Claims;

namespace BlueLagoon.Modules.Iam.Core.Services.Dto;

public class AuthorizationResultDto
{
    public bool IsChallenge { get; set; }

    public bool IsSignOut { get; set; } 

    public ClaimsPrincipal Principal { get; set; }

    public string RedirectUri { get; set; }
}
