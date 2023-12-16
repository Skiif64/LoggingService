using FluentAssertions;
using LoggingService.Application.Auth.UnitTests.MockData;
using LoggingService.Application.Auth.UnitTests.TestBases;
using LoggingService.Application.Authentication.Application;

namespace LoggingService.Application.Auth.UnitTests.Tests.AuthenticationService;
public sealed class RenewTests : TestBase
{
    private readonly Mock<IApplicationIdentityRepository> _identityRepositoryMock;
    private readonly ApplicationAuthenticationService _sut;
    public RenewTests()
    {
        _identityRepositoryMock = new Mock<IApplicationIdentityRepository>();
        _sut = new ApplicationAuthenticationService(_identityRepositoryMock.Object, UnitOfWorkMock.Object);
    }

    [Fact]
    public async Task Renew_ShouldUpdateIdentity_WhenIdentityIsExists()
    {
        var identity = MockApplicationIdentity.Create("TestApiKey");
        var oldApiKey = identity.ApiKey;
        _identityRepositoryMock.Setup(x => x.GetByIdAsync(identity.Id, CancellationToken))
            .ReturnsAsync(identity);

        var result = await _sut.RenewApplicationAsync(identity.Id, DateTime.UtcNow.AddYears(1), CancellationToken);

        UnitOfWorkMock.Verify(x => x.SaveChangesAsync(CancellationToken), Times.Once);
        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().NotBeNullOrWhiteSpace();
        identity.ApiKey.Should().NotBe(oldApiKey);
    }

    [Fact]
    public async Task Renew_ShouldReturnNotFoundError_WhenIdentityNotExists()
    {
        var result = await _sut.RenewApplicationAsync(Guid.NewGuid(), DateTime.UtcNow.AddYears(1), CancellationToken);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Should().Be(ApplicationIdentityErrors.NotFound);
    }

    [Fact]
    public async Task Renew_ShouldReturnArgumentError_WhenExpirationIsLessThanCurrentTime()
    {
        var identityId = Guid.NewGuid();

        var result = await _sut.RenewApplicationAsync(identityId, DateTime.UtcNow.AddYears(-1), CancellationToken);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Should().Be(ApplicationIdentityErrors.ArgumentError);
    }
}
