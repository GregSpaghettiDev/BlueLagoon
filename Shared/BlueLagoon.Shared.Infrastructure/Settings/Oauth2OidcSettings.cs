namespace BlueLagoon.Shared.Infrastructure.Settings;

public sealed class Oauth2OidcSettings
{
    public Oauth2Endpoints Endpoints { get; init; }

    public string SigningKey { get; init; }

    public string EncryptionKey { get; init; }
}

public class Oauth2Endpoints
{
    public string AuthorizationEndpoint { get; init; }

    public string LogoutEndpoint { get; init; }

    public string TokenEndpoint { get; init; }
}