using LoggingService.Endpoints.Application.Endpoints;
using Microsoft.AspNetCore.Builder;

namespace LoggingService.Endpoints.Application;
public static class DependencyInjection
{
    public static WebApplication UseApplicationEndpoints(this WebApplication app)
    {
        var logEventEndpoints = new ApplicationLogEventEndpoints();
        var eventCollectionEndpoints = new ApplicationEventCollectionEndpoints();
        logEventEndpoints.ConfigureEndpoints(app);
        eventCollectionEndpoints.ConfigureEndpoints(app);
        return app;
    }
}
