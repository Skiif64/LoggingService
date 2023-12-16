using FluentAssertions;
using LoggingService.Application.Auth.UnitTests.TestBases;
using LoggingService.Application.Authentication.Application;

namespace LoggingService.Application.Auth.UnitTests.Tests.AuthenticationService;
public sealed class RegisterTests : TestBase
{
    private readonly Mock<IApplicationIdentityRepository> _identityRepositoryMock;
    private readonly ApplicationAuthenticationService _sut;
    public RegisterTests()
    {
        _identityRepositoryMock = new Mock<IApplicationIdentityRepository>();
        _sut = new ApplicationAuthenticationService(_identityRepositoryMock.Object, UnitOfWorkMock.Object);
    }

    [Fact]
    public async Task Register_ShouldReturnResultWithValidApiKey()
    {
        var result = await _sut.RegisterApplicationAsync("TestApplication", DateTime.UtcNow.AddYears(1), CancellationToken);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().NotBeNullOrWhiteSpace(); //TODO: validate key
    }

    [Fact]
    public async Task Register_ShouldReturnAlreadyExistsError_WhenDuplicateName()
    {
        var name = "TestApplication";
        _identityRepositoryMock.Setup(x => x.ExistsByNameAsync(name, CancellationToken))
            .ReturnsAsync(true);

        var result = await _sut.RegisterApplicationAsync(name, DateTime.UtcNow.AddYears(1), CancellationToken);

        result.IsSuccess.Should().BeFalse();
        result.Error!.Should().Be(ApplicationIdentityErrors.AlreadyExistsError(name));
    }
}
