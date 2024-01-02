using LoggingService.Domain.Features.LogEvents;
using Microsoft.Extensions.DependencyInjection;
using AutoFixture.Xunit2;
using System.ComponentModel.DataAnnotations;
using LoggingService.DataAccess.Ef.IntegrationTest.Fixtures;
using LoggingService.DataAccess.Ef.IntegrationTest.TestBases;
using LoggingService.Tests.Shared.Specimen;

namespace LoggingService.DataAccess.Ef.IntegrationTest.Tests;
public class LogEventRepositoryTests : TestBase
{
    private readonly ILogEventRepository _sut;
    public LogEventRepositoryTests(ApplicationFixture application)
        : base(application)
    {
        _sut = ScopeProvider.GetRequiredService<ILogEventRepository>();
    }

    [Fact]
    public async Task GetPagedByCollectionId_ShouldReturnOrderedByDescendingByTimestamp()
    {
        
        var collectionId = Guid.NewGuid();
        Fixture.Customizations.Add(new LogEventSpecimenBuilder(collectionId));
        var logs = Fixture.CreateMany<LogEvent>(20);
        await SeedAsync(logs);

        var pagedList = await _sut.GetPagedByCollectionIdAsync(collectionId, 0, 20, CancellationToken);

        pagedList.Items.Should().NotBeEmpty();
        pagedList.Items.Should().BeInDescendingOrder(log => log.Timestamp);
    }

    [Fact]
    public async Task InsertAsync_ShouldAddLogToDatabase()
    {
        Fixture.Customizations.Add(new LogEventSpecimenBuilder());
        var log = Fixture.Create<LogEvent>();

        await _sut.InsertAsync(log, CancellationToken);
        await Context.SaveChangesAsync(CancellationToken);

        var actualLog = await GetFirstFromDbAsync<LogEvent>(actual => actual.Id == log.Id);
        actualLog.Should().NotBeNull();
    }

    [Fact]
    public async Task InsertManyAsync_ShouldAddLogsToDatabase()
    {
        Fixture.Customizations.Add(new LogEventSpecimenBuilder());
        var logs = Fixture.CreateMany<LogEvent>(20);

        await _sut.InsertManyAsync(logs, CancellationToken);
        await Context.SaveChangesAsync(CancellationToken);

        var actualLogs = GetFromDb<LogEvent>(log => logs.Select(x => x.Id).Contains(log.Id));
        actualLogs.Should().HaveCount(logs.Count());
    }
}