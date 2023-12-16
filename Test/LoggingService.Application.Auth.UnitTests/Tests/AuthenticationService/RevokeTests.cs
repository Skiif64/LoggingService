using FluentAssertions;
using LoggingService.Application.Auth.UnitTests.MockData;
using LoggingService.Application.Auth.UnitTests.TestBases;
using LoggingService.Application.Authentication.Application;

namespace LoggingService.Application.Auth.UnitTests.Tests.AuthenticationService;
public sealed class RevokeTests : TestBase
{
    private readonly Mock<IApplicationIdentityRepository> _identityRepositoryMock;
    private readonly ApplicationAuthenticationService _sut;
    public RevokeTests()
    {
        _identityRepositoryMock = new Mock<IApplicationIdentityRepository>();
        _sut = new ApplicationAuthenticationService(_identityRepositoryMock.Object, UnitOfWorkMock.Object);
    }

    [Fact]
    public async Task Revoke_ShouldRemoveApplication()
    {
        var identity = MockApplicationIdentity.Create("TestApiKey");
        _identityRepositoryMock.Setup(x => x.GetByIdAsync(identity.Id, CancellationToken))
            .ReturnsAsync(identity);
        var result = await _sut.RevokeApplicationAsync(identity.Id, CancellationToken);

        result.IsSuccess.Should().BeTrue();
        _identityRepositoryMock.Verify(x => x.Delete(identity), Times.Once);
        UnitOfWorkMock.Verify(x => x.SaveChangesAsync(CancellationToken), Times.Once);
    }

    [Fact]
    public async Task Revoke_ShouldReturnNotFoundResult_WhenApplicationNotExists()
    {
        var identityId = Guid.NewGuid();

        var result = await _sut.RevokeApplicationAsync(identityId, CancellationToken);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Should().Be(ApplicationIdentityErrors.NotFound);
    }
}
