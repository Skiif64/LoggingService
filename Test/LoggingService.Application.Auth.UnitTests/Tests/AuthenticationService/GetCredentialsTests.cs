using FluentAssertions;
using LoggingService.Application.Auth.UnitTests.MockData;
using LoggingService.Application.Auth.UnitTests.TestBases;
using LoggingService.Application.Authentication.Application;

namespace LoggingService.Application.Auth.UnitTests.Tests.AuthenticationService;
public sealed class GetCredentialsTests : TestBase
{
    private readonly Mock<IApplicationIdentityRepository> _identityRepositoryMock;
    private readonly ApplicationAuthenticationService _sut;
    public GetCredentialsTests()
    {
        _identityRepositoryMock = new Mock<IApplicationIdentityRepository>();
        _sut = new ApplicationAuthenticationService(_identityRepositoryMock.Object, UnitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetCredentials_ShouldReturnSuccessResult_WhenApiKeyIsValid()
    {
        var key = $"key.{Guid.NewGuid()}";
        var appIdentity = MockApplicationIdentity.Create(key);
        _identityRepositoryMock.Setup(x => x.GetByKeyPrefixAsync(It.IsAny<string>(), CancellationToken))
            .ReturnsAsync(appIdentity);

        var result = await _sut.GetApplicationAsync(key, CancellationToken);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetCredentials_ShouldReturnKeyFailureResult_WhenApiKeyIsInvalid()
    {
        var key = $"key.{Guid.NewGuid()}";
        var appIdentity = MockApplicationIdentity.Create("Some.InvalidKey");
        _identityRepositoryMock.Setup(x => x.GetByKeyPrefixAsync(It.IsAny<string>(), CancellationToken))
            .ReturnsAsync(appIdentity);

        var result = await _sut.GetApplicationAsync(key, CancellationToken);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ApplicationIdentityErrors.KeyError);
    }
}
