using LoggingService.DataAccess.Postgres.IntegrationTest.Fixtures;
using LoggingService.Domain.Features.LogEvents;
using Microsoft.Extensions.DependencyInjection;
using AutoFixture.Xunit2;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;

namespace LoggingService.DataAccess.Postgres.IntegrationTest.Tests;
public class LogEventRepositoryTests : TestBase
{
    public LogEventRepositoryTests(ApplicationFixture application) 
        : base(application)
    {
    }

    [Theory, InlineAutoData]
    public async Task GetPagedByCollectionId_ShouldReturnCorrectPagedList_WhenCountAlwaysEqualPageSize([Range(0, 10)]int pageIndex)
    {
        var pageSize = 20;
        var count = (pageIndex + 1) * pageSize;
        var collectionId = Guid.NewGuid();
        var logs = Fixture.Build<LogEvent>()
            .With(prop => prop.CollectionId, collectionId)
            .CreateMany(count);
        await SeedAsync(logs);
        var sut = ScopeProvider.GetRequiredService<ILogEventRepository>();

        var pagedList = await sut.GetPagedByCollectionIdAsync(collectionId, pageIndex, pageSize, CancellationToken);

        pagedList.Items.Should().NotBeEmpty();
        pagedList.Items.Should().Contain(log => log.CollectionId == collectionId);
        pagedList.CurrentCount.Should().Be(pageSize);
    }

    [Fact]
    public async Task GetPagedByCollectionId_ShouldReturnOrderedByDescendingByTimestamp()
    {
        var collectionId = Guid.NewGuid();
        var logs = Fixture.Build<LogEvent>()
            .With(prop => prop.CollectionId, collectionId)
            .CreateMany(20);
        await SeedAsync(logs);
        var sut = ScopeProvider.GetRequiredService<ILogEventRepository>();

        var pagedList = await sut.GetPagedByCollectionIdAsync(collectionId, 0, 20, CancellationToken);

        pagedList.Items.Should().NotBeEmpty();
        pagedList.Items.Should().BeInDescendingOrder(log => log.Timestamp);
    }
}
