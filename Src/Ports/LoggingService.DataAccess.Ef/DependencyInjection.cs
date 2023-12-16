using LoggingService.Application.Authentication.Application;
using LoggingService.Application.Base;
using LoggingService.DataAccess.Ef.Repositories;
using LoggingService.Domain.Features.EventCollections;
using LoggingService.Domain.Features.LogEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoggingService.DataAccess.Ef;
public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Npgsql");
        if (connectionString is null)
        {
            throw new ArgumentNullException("Connection string with key: Npgsql not found");
        }
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(connectionString);
        });

        services.AddScoped<ILogEventRepository, LogEventRepository>();
        services.AddScoped<IEventCollectionRepository, EventCollectionRepository>();
        services.AddScoped<IApplicationIdentityRepository, ApplicationIdentityRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        var provider = services.BuildServiceProvider();
        using (var scope = provider.CreateScope())
        {
            var context = provider.GetRequiredService<ApplicationDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

        return services;
    }
}
