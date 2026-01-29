namespace BlueLagoon.Shared.Infrastructure.Settings;

public sealed class SwaggerSettings
{
    public string OauthClientId { get; init; }

    public string OauthClientSecret { get; init; }
    
    public string OauthRelativeRedirectUri { get; init; }
    
    public string RelativeOpenApiPath { get; init; }

    public string RelativeUiPath { get; init; }

    public string Name { get; init;  }

    public string Description { get; init; }
}