using LoggingService.Application.Features.LogEvents.Commands.Create;
using LoggingService.Application.Features.LogEvents.Commands.CreateBatched;
using LoggingService.Application.Features.LogEvents.Commands;
using LoggingService.WebApi.Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MapsterMapper;

namespace LoggingService.Endpoints.Application.Endpoints;
public sealed class ApplicationLogEventEndpoints
{    
    public void ConfigureEndpoints(WebApplication app)
    {
        app.MapPost("~/api/event/{collectionName}", InsertAsync);
        app.MapPost("~/api/event/{collectionName}/batched", InsertBatchedAsync);
    }

    private async Task<IResult> InsertAsync(HttpContext httpContext,
        [FromServices] ISender sender,
        [FromServices] IMapper mapper,
        [FromRoute] string collectionName,
        [FromBody] CreateLogEventViewModel model,
        CancellationToken cancellationToken)
    {
        var command = new CreateLogEventCommand(collectionName, mapper.Map<CreateLogEventDto>(model));
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

    private async Task<IResult> InsertBatchedAsync(HttpContext httpContext,
        [FromServices] ISender sender,
        [FromServices] IMapper mapper,
        [FromRoute] string collectionName,
        [FromBody] IEnumerable<CreateLogEventViewModel> models,
        CancellationToken cancellationToken)
    {
        var command = new CreateLogEventBatchedCommand(collectionName,
            mapper.Map<IEnumerable<CreateLogEventDto>>(models));
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
