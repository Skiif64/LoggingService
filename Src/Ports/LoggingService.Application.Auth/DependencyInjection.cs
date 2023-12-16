using LoggingService.Application.Authentication.Application;
using Microsoft.Extensions.DependencyInjection;

namespace LoggingService.Application.Auth;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services)
    {
        services.AddScoped<IApplicationAuthenticationService, ApplicationAuthenticationService>();
        return services;
    }
}
