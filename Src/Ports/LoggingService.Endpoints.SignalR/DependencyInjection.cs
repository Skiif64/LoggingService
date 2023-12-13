using LoggingService.Endpoints.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LoggingService.Endpoints.SignalR;
public static class DependencyInjection
{
    public static IServiceCollection AddHubs(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddMediatR(cfg => //TODO: add only notificationHandlers
        {
            cfg.RegisterServicesFromAssemblyContaining<NotificationHub>();
        });
        return services;
    }

    public static WebApplication UseHubs(this WebApplication app)
    {
        app.MapHub<NotificationHub>("hub/notification");
        return app;
    }
}
