﻿using LoggingService.Domain.Features.EventCollections;
using LoggingService.Domain.Features.LogEvents;
using Microsoft.EntityFrameworkCore;

namespace LoggingService.DataAccess.Postgres;
public sealed class ApplicationDbContext : DbContext
{
    public DbSet<LogEvent> LogEvents { get; private set; }
    public DbSet<EventCollection> EventCollections { get; private set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}