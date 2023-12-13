using LoggingService.Domain.Features.LogEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoggingService.DataAccess.Ef.EntityConfiguration;
internal sealed class LogEventConfiguration
    : IEntityTypeConfiguration<LogEvent>
{
    public void Configure(EntityTypeBuilder<LogEvent> builder)
    {
        builder.HasIndex(prop => prop.CollectionId);
    }
}
