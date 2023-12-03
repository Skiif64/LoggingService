using AutoFixture.AutoMoq;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using LoggingService.DataAccess.Postgres.IntegrationTest.Fixtures;
using LoggingService.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

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

    public async Task<TEntity?> GetFirstFromDbAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : BaseEntity
    {
        await using var scope = Application.Provider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return await context.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }

    public IEnumerable<TEntity> GetFromDb<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : BaseEntity
    {
        using var scope = Application.Provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return context.Set<TEntity>().Where(predicate).ToList();
    }

    public Task InitializeAsync() 
        => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await Application.ResetAsync();
        _scope.Dispose();
    }
}
