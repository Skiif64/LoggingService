using FluentAssertions;
using LoggingService.Application.Auth.UnitTests.TestBases;
using LoggingService.Application.Authentication.Application;
using LoggingService.Domain.Features.Applications;
using ApplicationIdentity = LoggingService.Domain.Features.Applications.ApplicationIdentity;

namespace LoggingService.Application.Auth.UnitTests.Tests.AuthenticationService;
public sealed class RevokeTests : TestBase
{
    private const string ValidPrefix = "TestPrefix";
    private const string ValidSecret = "SomeRandomValueThatHasToBeNotHere";
    private static readonly DateTime Expiration = DateTime.UtcNow.AddYears(1);
    
    private readonly Mock<IApplicationIdentityRepository> _identityRepositoryMock;
    private readonly Mock<IApiKeyRepository> _apiKeyRepositoryMock;
    private readonly ApplicationAuthenticationService _sut;
    public RevokeTests()
    {
        _identityRepositoryMock = new Mock<IApplicationIdentityRepository>();
        _apiKeyRepositoryMock = new Mock<IApiKeyRepository>();
        _sut = new ApplicationAuthenticationService(_identityRepositoryMock.Object, _apiKeyRepositoryMock.Object, UnitOfWorkMock.Object);
    }

    [Fact]
    public async Task Revoke_ShouldRemoveApiKey()
    {
        var identity = ApplicationIdentity.Create("TestApplication");
        var apiKey = ApiKey.Create(identity.Id, ValidPrefix, ValidSecret, Expiration);
        
        _apiKeyRepositoryMock.Setup(x => x.GetByPrefixAsync(ValidPrefix, CancellationToken))
            .ReturnsAsync(apiKey);
        
        var result = await _sut.RevokeApiKeyAsync(ValidPrefix, CancellationToken);

        result.IsSuccess.Should().BeTrue();
        _apiKeyRepositoryMock.Verify(x => x.Delete(apiKey), Times.Once);
    }

    [Fact]
    public async Task Revoke_ShouldReturnNotFoundResult_WhenApplicationNotExists()
    {
        var result = await _sut.RevokeApiKeyAsync(ValidPrefix, CancellationToken);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Should().Be(ApplicationIdentityErrors.NotFound);
    }
}
