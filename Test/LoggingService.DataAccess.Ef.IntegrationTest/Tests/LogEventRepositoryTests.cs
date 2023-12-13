using LoggingService.DataAccess.Postgres.IntegrationTest.Fixtures;
using LoggingService.Domain.Features.LogEvents;
using Microsoft.Extensions.DependencyInjection;
using AutoFixture.Xunit2;
using System.ComponentModel.DataAnnotations;

namespace LoggingService.DataAccess.Postgres.IntegrationTest.Tests;
public class LogEventRepositoryTests : TestBase
{
    private readonly ILogEventRepository _sut;
    public LogEventRepositoryTests(ApplicationFixture application) 
        : base(application)
    {
        _sut = ScopeProvider.GetRequiredService<ILogEventRepository>();
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

        var pagedList = await _sut.GetPagedByCollectionIdAsync(collectionId, pageIndex, pageSize, CancellationToken);

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

        var pagedList = await _sut.GetPagedByCollectionIdAsync(collectionId, 0, 20, CancellationToken);

        pagedList.Items.Should().NotBeEmpty();
        pagedList.Items.Should().BeInDescendingOrder(log => log.Timestamp);
    }

    [Fact]
    public async Task InsertAsync_ShouldAddLogToDatabase()
    {
        var log = Fixture.Create<LogEvent>();

        await _sut.InsertAsync(log, CancellationToken);
        await Context.SaveChangesAsync(CancellationToken);

        var actualLog = await GetFirstFromDbAsync<LogEvent>(actual => actual.Id == log.Id);
        actualLog.Should().NotBeNull();
    }

    [Fact]
    public async Task InsertManyAsync_ShouldAddLogsToDatabase()
    {
        var logs = Fixture.CreateMany<LogEvent>(20);

        await _sut.InsertManyAsync(logs, CancellationToken);
        await Context.SaveChangesAsync(CancellationToken);

        var actualLogs = GetFromDb<LogEvent>(log => logs.Select(x => x.Id).Contains(log.Id));
        actualLogs.Should().HaveCount(logs.Count());
    }
}
