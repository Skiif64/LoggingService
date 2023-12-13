using LoggingService.Application.Features.EventCollections.Commands.Queries;
using LoggingService.Application.Features.LogEvents.Queries.GetPaged;
using LoggingService.WebApi.Contracts.Common;
using LoggingService.WebApi.Contracts.Models;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoggingService.Endpoints.Frontend.Endpoints;
public sealed class FrontendEventCollectionEndpoints
{
    public void ConfigureEndpoints(WebApplication app)
    {
        app.MapGet("~/api/collection", GetPagedAsync);
    }

    private async Task<IResult> GetPagedAsync(HttpContext httpContext,
        [FromServices] ISender sender,
        [FromServices] IMapper mapper,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPagedEventCollectionQuery(pageIndex, pageSize);

        var result = await sender.Send(query, cancellationToken);
        if (result.IsSuccess)
        {
            var mapped = mapper.Map<PagedListViewModel<EventCollectionViewModel>>(result.Value!);
            return Results.Ok(mapped);
        }
        else
        {
            return Results.BadRequest(result.Error);
        }
    }
}
