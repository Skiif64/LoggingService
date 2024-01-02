using LoggingService.Application.Authentication;
using LoggingService.Application.Authentication.Application;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace LoggingService.Application.Auth;
public sealed class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private const string AuthKeyHeader = "X-Api-Key";
    private readonly IApplicationAuthenticationService _authService;
    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IApplicationAuthenticationService authService)
        : base(options, logger, encoder, clock)
    {
        _authService = authService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if(!Context.Request.Headers.TryGetValue(AuthKeyHeader, out var key) || (string?)key is null)
        {
            return AuthenticateResult.Fail($"Header {AuthKeyHeader} not found or key is empty.");
        }

        var getCredentialsResult = await _authService.GetApplicationAsync(key!);
        if(!getCredentialsResult.IsSuccess)
        {
            return AuthenticateResult.Fail(getCredentialsResult.Error!.ToString());
        }
        var identity = new ClaimsIdentity(getCredentialsResult.Value!.GetClaims(), AuthenticationSchemes.ApiKeyScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationSchemes.ApiKeyScheme);
        //TODO: log success
        return AuthenticateResult.Success(ticket);
    }
}

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
}