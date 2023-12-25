using FluentAssertions;
using LoggingService.Application.Auth.UnitTests.TestBases;
using LoggingService.Application.Authentication.Application;
using LoggingService.Domain.Features.Applications;
using ApplicationIdentity = LoggingService.Domain.Features.Applications.ApplicationIdentity;

namespace LoggingService.Application.Auth.UnitTests.Tests.AuthenticationService;
public sealed class GetCredentialsTests : TestBase
{
    private const string ValidPrefix = "TestPrefix";
    private const string ValidSecret = "SomeRandomValueThatHasToBeNotHere";
    private const string ValidApiKey = $"{ValidPrefix}.{ValidSecret}";
    
    private readonly Mock<IApplicationIdentityRepository> _identityRepositoryMock;
    private readonly Mock<IApiKeyRepository> _apiKeyRepositoryMock;
    private readonly ApplicationAuthenticationService _sut;

    
    public GetCredentialsTests()
    {
        _identityRepositoryMock = new Mock<IApplicationIdentityRepository>();
        _apiKeyRepositoryMock = new Mock<IApiKeyRepository>();
        _sut = new ApplicationAuthenticationService(_identityRepositoryMock.Object,
            _apiKeyRepositoryMock.Object,
            UnitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetCredentials_ShouldReturnSuccessResult_WhenApiKeyIsValid()
    {
        var appIdentity = ApplicationIdentity.Create("TestApplication");
        var apiKey = ApiKey.Create(appIdentity.Id, ValidPrefix, ValidSecret, DateTime.UtcNow.AddYears(1));
        _identityRepositoryMock.Setup(x => x.GetByIdAsync(appIdentity.Id, CancellationToken))
            .ReturnsAsync(appIdentity);
        _apiKeyRepositoryMock.Setup(x => x.GetByPrefixAsync(ValidPrefix, CancellationToken))
            .ReturnsAsync(apiKey);
        
        var result = await _sut.GetApplicationAsync(ValidApiKey, CancellationToken);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetCredentials_ShouldReturnKeyFailureResult_WhenApiKeyIsInvalid()
    {
        var invalidKey = $"{ValidPrefix}.SomeInvalidSecret";
        var appIdentity = ApplicationIdentity.Create("TestApplication");
        var apiKey = ApiKey.Create(appIdentity.Id, ValidPrefix, ValidSecret, DateTime.UtcNow.AddYears(1));
        _apiKeyRepositoryMock.Setup(x => x.GetByPrefixAsync(ValidPrefix, CancellationToken))
            .ReturnsAsync(apiKey);
        var result = await _sut.GetApplicationAsync(invalidKey, CancellationToken);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ApplicationIdentityErrors.KeyError);
    }
}
