using LoggingService.Application.Authentication.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoggingService.DataAccess.Ef.EntityConfiguration;
internal sealed class ApplicationIdentityConfiguration
    : IEntityTypeConfiguration<ApplicationIdentity>
{
    public void Configure(EntityTypeBuilder<ApplicationIdentity> builder)
    {
        builder.HasIndex(prop => prop.Name).IsUnique();
        builder.OwnsOne(prop => prop.ApiKey);
    }
}
