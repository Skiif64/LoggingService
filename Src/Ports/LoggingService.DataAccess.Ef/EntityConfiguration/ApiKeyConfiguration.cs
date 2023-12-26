using LoggingService.Application.Authentication.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoggingService.DataAccess.Ef.EntityConfiguration;

public class ApiKeyConfiguration : IEntityTypeConfiguration<ApiKey>
{
    public void Configure(EntityTypeBuilder<ApiKey> builder)
    {
        builder.HasIndex(prop => prop.ApiKeyPrefix).IsUnique();
        builder.HasIndex(prop => prop.ApplicationId).IsUnique();
    }
}