using LoggingService.Application.Authentication.Application;
using LoggingService.DataAccess.Ef.IntegrationTest.Fixtures;
using LoggingService.DataAccess.Ef.IntegrationTest.TestBases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LoggingService.DataAccess.Ef.IntegrationTest.Tests;
public class ApplicationIdentityRepositoryTests : TestBase
{
    private readonly IApplicationIdentityRepository _sut;
    public ApplicationIdentityRepositoryTests(ApplicationFixture application) : base(application)
    {
        _sut = ScopeProvider.GetRequiredService<IApplicationIdentityRepository>();
    }

    [Fact]
    public async Task InsertAsync_ShouldAddToDatabase()
    {
        var identity = MockApplicationIdentity.Create("Key.SomeHashCode");
       
        await _sut.InsertAsync(identity, CancellationToken);
        await Context.SaveChangesAsync(CancellationToken);

        var actual = await Context.Applications
            .FirstOrDefaultAsync(app => app.ApiKey.ApiKeyPrefix == identity.ApiKey.ApiKeyPrefix);

        actual.Should().NotBeNull("identity was not added to database");
    }

    [Fact]
    public async Task GetByPrefixKey_ShouldReturnIdentity_WhenIdentityExists()
    {
        var identity = MockApplicationIdentity.Create("Key.SomeHashCode");
        await SeedAsync(identity);

        var actual = await _sut.GetByKeyPrefixAsync(identity.ApiKey.ApiKeyPrefix, CancellationToken);
        
        actual.Should().NotBeNull("Identity with prefix key not found");
    }

    [Fact]
    public async Task GetByPrefixKey_ShouldReturnNull_WhenKeyWithPrefixDoesNotExists()
    {
        var identity = MockApplicationIdentity.Create("Key.SomeHashCode");
        await SeedAsync(identity);

        var actual = await _sut.GetByKeyPrefixAsync("Kkkkey", CancellationToken);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task ExistsByName_ShouldReturnTrue_WhenIdentityWithNameExists()
    {
        var identity = MockApplicationIdentity.Create("Key.SomeHashCode");
        await SeedAsync(identity);

        var actual = await _sut.ExistsByNameAsync(identity.Name, CancellationToken);

        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByName_ShouldReturnFalse_WhenIdentityWithNameNotExists()
    {      
        var actual = await _sut.ExistsByNameAsync("Key", CancellationToken);

        actual.Should().BeFalse();
    }
}
