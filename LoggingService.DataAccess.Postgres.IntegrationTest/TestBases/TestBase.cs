using AutoFixture.AutoMoq;
using LoggingService.DataAccess.Postgres.IntegrationTest.Fixtures;
using LoggingService.Domain.Base;
using Microsoft.Extensions.DependencyInjection;

namespace LoggingService.DataAccess.Postgres.IntegrationTest.TestBases;
public abstract class TestBase : IAsyncLifetime, IClassFixture<ApplicationFixture>
{
    private readonly IServiceScope _scope;
    protected ApplicationFixture Application { get; }
    protected CancellationToken CancellationToken { get; }
    protected IFixture Fixture { get; }
    protected IServiceProvider ScopeProvider { get; }
    protected ApplicationDbContext Context { get; }

    protected TestBase(ApplicationFixture application)
    {
        Application = application;
        CancellationToken = new CancellationToken();
        Fixture = new Fixture().Customize(new AutoMoqCustomization()).Customize(new DateTimeCustomization());
        _scope = Application.Provider.CreateScope();
        ScopeProvider = _scope.ServiceProvider;
        Context = ScopeProvider.GetRequiredService<ApplicationDbContext>();
    }

    public Task SeedAsync<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : BaseEntity
        => SeedAsync(entities.ToArray());

    public async Task SeedAsync<TEntity>(params TEntity[] entities)
        where TEntity : BaseEntity
    {
        await using var scope = Application.Provider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Set<TEntity>().AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public Task InitializeAsync() 
        => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await Application.ResetAsync();
        _scope.Dispose();
    }
}
