using LoggingService.Application.Auth.Endpoints;
using LoggingService.Application.Authentication.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LoggingService.Application.Auth;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services)
    {
        services.AddScoped<IApplicationAuthenticationService, ApplicationAuthenticationService>();
        return services;
    }

    public static WebApplication UseApplicationAuthenticationEndpoints(this WebApplication app)
    {
        var endpoints = new ApplicationAuthEndpoints();
        endpoints.ConfigureEndpoints(app);
        return app;
    }
}
