using LoggingService.Application.Authentication.Application;
using LoggingService.DataAccess.Ef;
using LoggingService.DataAccess.Ef.Repositories;
using LoggingService.Domain.Features.EventCollections;
using LoggingService.Domain.Features.LogEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using System.Data.Common;
using Testcontainers.PostgreSql;

namespace LoggingService.DataAccess.Ef.IntegrationTest.Fixtures;
public sealed class ApplicationFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres;
    private Respawner _respawner = null!;
    private DbConnection _connection = null!;
    public IServiceProvider Provider { get; private set; } = null!;
    public ApplicationFixture()
    {
        _postgres = new PostgreSqlBuilder()
            .WithDatabase("Logs-Test")
            .WithName($"Postgres-Test-{Guid.NewGuid()}")
            .Build();
    }
    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
        _connection = new NpgsqlConnection(_postgres.GetConnectionString());
        await _connection.OpenAsync();
        await SetupServicesAsync();
        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[]
                {
                    "public"
                },
        });
    }

    public async Task ResetAsync()
    {
        await _respawner.ResetAsync(_connection);
    }

    private async Task SetupServicesAsync()
    {
        var collection = new ServiceCollection();
        collection.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(_postgres.GetConnectionString());
        });

        collection.AddScoped<ILogEventRepository, LogEventRepository>();
        collection.AddScoped<IEventCollectionRepository, EventCollectionRepository>();
        collection.AddScoped<IApplicationIdentityRepository, ApplicationIdentityRepository>();
        Provider = collection.BuildServiceProvider();
        await using var scope = Provider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgres.DisposeAsync();
    }
}
