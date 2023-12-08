using LoggingService.Application.Base.Messaging;
using LoggingService.Domain.Features.LogEvents;
using Microsoft.Extensions.DependencyInjection;

namespace LoggingService.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });
        services.AddScoped<IEventBus, EventBus.EventBus>();
        services.AddScoped<ILogEventService, LogEventService>();
        return services;
    }
}
