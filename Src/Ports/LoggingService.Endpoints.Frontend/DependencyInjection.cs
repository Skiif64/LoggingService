using LoggingService.Endpoints.Frontend.Endpoints;
using Microsoft.AspNetCore.Builder;

namespace LoggingService.Endpoints.Frontend;
public static class DependencyInjection
{
    public static WebApplication UseFrontendEndpoints(this WebApplication app)
    {
        var eventCollectionEndpoints = new FrontendEventCollectionEndpoints();
        var logEventEndpoints = new FrontendLogEventEndpoints();
        eventCollectionEndpoints.ConfigureEndpoints(app);
        logEventEndpoints.ConfigureEndpoints(app);
        return app;
    }
}
