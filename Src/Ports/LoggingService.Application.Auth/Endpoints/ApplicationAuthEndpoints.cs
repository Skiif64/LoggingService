using LoggingService.Application.Authentication.Application;
using LoggingService.WebApi.Contracts.Models.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoggingService.Application.Auth.Endpoints;
public sealed class ApplicationAuthEndpoints
{
    public void ConfigureEndpoints(WebApplication app)
    {
        app.MapPost("~/auth/application/register", RegisterAsync);
        app.MapPut("~/auth/application/renew", RenewAsync);
        app.MapDelete("~/auth/application/revoke", RevokeAsync);
        //TODO: get paged
    }
    private async Task<IResult> RegisterAsync(HttpContext context,
        [FromServices] IApplicationAuthenticationService service,
        [FromBody] ApplicationRegisterViewModel model,
        CancellationToken cancellationToken)
    {
        var result = await service.RegisterApplicationAsync(model.Name, model.ExpireAtUtc, cancellationToken);
        if(result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }
        else
        {
            return Results.BadRequest(result.Error);
        }
    }

    private async Task<IResult> RenewAsync(HttpContext context,
        [FromServices] IApplicationAuthenticationService service,
        [FromBody] ApplicationRenewViewModel model,
        CancellationToken cancellationToken)
    {
        var result = await service.RenewApiKeyAsync(model.ApiKeyPrefix, model.ExpireAtUtc, cancellationToken);
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }
        else
        {
            return Results.BadRequest(result.Error);
        }
    }

    private async Task<IResult> RevokeAsync(HttpContext context,
        [FromServices] IApplicationAuthenticationService service,
        [FromQuery] string keyPrefix,
        CancellationToken cancellationToken)
    {
        var result = await service.RevokeApiKeyAsync(keyPrefix, cancellationToken);
        if (result.IsSuccess)
        {
            return Results.Ok();
        }
        else
        {
            return Results.BadRequest(result.Error);
        }
    }
}
