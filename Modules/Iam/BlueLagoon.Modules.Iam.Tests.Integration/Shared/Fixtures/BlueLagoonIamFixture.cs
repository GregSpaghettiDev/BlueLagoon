using DotNet.Testcontainers.Builders;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace BlueLagoon.Modules.Iam.Tests.Integration.Shared.Fixtures;

public sealed class BlueLagoonIamFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _pgSqlContainer =
        new PostgreSqlBuilder("postgres:17")
        .WithAutoRemove(true)
        .WithPortBinding(5432, 5432)
        .WithExposedPort(5432)
        .WithHostname("localhost")
        .WithDatabase("blue_lagoon")
        .WithUsername("blue_owner")
        .WithPassword("HannahMontana@1@")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilExternalTcpPortIsAvailable(5432))
        .Build();

    public readonly Guid FixtureVersionId = Guid.NewGuid();

    public string SqlServerContainerId => _pgSqlContainer.Id;

    public IConfiguration Configuration { get; private set; }

    public async ValueTask InitializeAsync()
    {
        await _pgSqlContainer.StartAsync();
        await _pgSqlContainer.ExecScriptAsync(TestResources.IamDb);
    }

    public async ValueTask DisposeAsync()
    {
        await _pgSqlContainer.DisposeAsync();
    }
}
