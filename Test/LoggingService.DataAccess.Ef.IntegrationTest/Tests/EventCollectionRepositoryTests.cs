using LoggingService.DataAccess.Ef.IntegrationTest.Fixtures;
using LoggingService.DataAccess.Ef.IntegrationTest.TestBases;
using LoggingService.Domain.Features.EventCollections;
using Microsoft.Extensions.DependencyInjection;

namespace LoggingService.DataAccess.Ef.IntegrationTest.Tests;
public class EventCollectionRepositoryTests : TestBase
{
    private readonly IEventCollectionRepository _sut;
    public EventCollectionRepositoryTests(ApplicationFixture application) : base(application)
    {
        _sut = ScopeProvider.GetRequiredService<IEventCollectionRepository>();
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnValidPagedList_WhenCollectionsExists()
    {
        var collections = Fixture.CreateMany<EventCollection>(20);
        await SeedAsync(collections);
        var result = await _sut.GetPagedAsync(0, 20, CancellationToken);

        result.Items.Should().HaveCount(20);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnCollection_WhenCollectionWithNameContains()
    {
        var collection = Fixture.Create<EventCollection>();
        await SeedAsync(collection);

        var result = await _sut.GetByNameAsync(collection.Name, CancellationToken);

        result.Should().NotBeNull();
        result!.Name.Should().Be(collection.Name);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnNull_WhenCollectionWithNameDoesNotContains()
    {
        var result = await _sut.GetByNameAsync("Some Name", CancellationToken);

        result.Should().BeNull();
    }

    [Fact]
    public async Task ExistByNameAsync_ShouldReturnTrue_WhenCollectionWithNameExists()
    {
        var collection = Fixture.Create<EventCollection>();
        await SeedAsync(collection);

        var result = await _sut.ExistByNameAsync(collection.Name, CancellationToken);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistByNameAsync_ShouldReturnFalse_WhenCollectionDoesNotExists()
    {
        var result = await _sut.ExistByNameAsync("Some name", CancellationToken);

        result.Should().BeFalse();
    }
}
