using LoggingService.Application.Authentication.Application;
using LoggingService.Domain.Features.Applications;
using LoggingService.Domain.Features.EventCollections;
using LoggingService.Domain.Features.LogEvents;
using Microsoft.EntityFrameworkCore;

namespace LoggingService.DataAccess.Ef;
public sealed class ApplicationDbContext : DbContext
{
    public DbSet<LogEvent> LogEvents { get; private set; }
    public DbSet<EventCollection> EventCollections { get; private set; }
    public DbSet<ApplicationIdentity> Applications { get; private set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
