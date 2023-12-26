using LoggingService.Application.Authentication.Application;
using LoggingService.DataAccess.Ef.IntegrationTest.Fixtures;
using LoggingService.DataAccess.Ef.IntegrationTest.TestBases;
using Microsoft.Extensions.DependencyInjection;

namespace LoggingService.DataAccess.Ef.IntegrationTest.Tests;

public class ApiKeyRepositoryTests : TestBase
{
    private const string ValidPrefix = "TestPrefix";
    private const string ValidSecret = "SomeRandomValueThatHasToBeNotHere";
    private static readonly DateTime Expiration = DateTime.UtcNow.AddYears(1);
    
    private readonly IApiKeyRepository _sut;
    
    public ApiKeyRepositoryTests(ApplicationFixture application) : base(application)
    {
        _sut = ScopeProvider.GetRequiredService<IApiKeyRepository>();
    }

    [Fact]
    public async Task GetByPrefixAsync_ShouldReturnApiKey_WhenExists()
    {
        var key = ApiKey.Create(Guid.NewGuid(), ValidPrefix, ValidSecret, Expiration);
        await SeedAsync(key);

        var result = await _sut.GetByPrefixAsync(ValidPrefix, CancellationToken);

        result.Should().NotBeNull();

    }
}