using LoggingService.Application.Authentication.Application;
using LoggingService.DataAccess.Ef.IntegrationTest.Fixtures;
using LoggingService.DataAccess.Ef.IntegrationTest.TestBases;
using LoggingService.Domain.Features.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ApplicationIdentity = LoggingService.Domain.Features.Applications.ApplicationIdentity;

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
        var identity = ApplicationIdentity.Create("TestApplication");
       
        await _sut.InsertAsync(identity, CancellationToken);
        await Context.SaveChangesAsync(CancellationToken);

        var actual = await Context.Applications
            .FirstOrDefaultAsync(app => app.Id == identity.Id);

        actual.Should().NotBeNull("identity was not added to database");
    }

    [Fact]
    public async Task ExistsByName_ShouldReturnTrue_WhenIdentityWithNameExists()
    {
        var identity = ApplicationIdentity.Create("TestApplication");
        await SeedAsync(identity);

        var actual = await _sut.ExistsByNameAsync(identity.Name, CancellationToken);

        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByName_ShouldReturnFalse_WhenIdentityWithNameNotExists()
    {      
        var actual = await _sut.ExistsByNameAsync("NotExisting", CancellationToken);

        actual.Should().BeFalse();
    }
}
