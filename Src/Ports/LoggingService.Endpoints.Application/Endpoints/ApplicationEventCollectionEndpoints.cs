using LoggingService.Application.Authentication;
using LoggingService.Application.Features.EventCollections.Commands.Create;
using LoggingService.WebApi.Contracts.Models;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoggingService.Endpoints.Application.Endpoints;
public sealed class ApplicationEventCollectionEndpoints
{
    public void ConfigureEndpoints(WebApplication app)
    {
        app.MapPost("~/api/collection", CreateAsync);
    }

    [Authorize(AuthenticationSchemes = AuthenticationSchemes.ApiKeyScheme)]
    public async Task<IResult> CreateAsync(HttpContext httpContext,
        [FromServices] ISender sender,
        [FromBody] CreateEventCollectionViewModel model,
        CancellationToken cancellationToken)
    {
        var command = new CreateEventCollectionCommand(model.Name, Guid.NewGuid()); //TODO: get app id

        var result = await sender.Send(command, cancellationToken);
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
