using LoggingService.Domain.Features.EventCollections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoggingService.DataAccess.Postgres.EntityConfiguration;
internal sealed class EventCollectionConfiguration
    : IEntityTypeConfiguration<EventCollection>
{
    public void Configure(EntityTypeBuilder<EventCollection> builder)
    {
        builder.HasIndex(prop => prop.ApplicationId);
    }
}
