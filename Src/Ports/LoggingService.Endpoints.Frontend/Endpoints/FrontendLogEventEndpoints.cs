using LoggingService.Application.Features.LogEvents.Queries.GetPaged;
using LoggingService.WebApi.Contracts.Common;
using LoggingService.WebApi.Contracts.Models;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoggingService.Endpoints.Frontend.Endpoints;
public sealed class FrontendLogEventEndpoints
{
    public void ConfigureEndpoints(WebApplication app)
    {
        app.MapGet("~/api/event/{collectionName}", GetPagedAsync);
    }

    private async Task<IResult> GetPagedAsync(
        [FromServices] ISender sender,
        [FromServices] IMapper mapper,
        [FromRoute] string collectionName,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        //TODO: validate page index & size
        var query = new GetPagedLogEventsQuery(collectionName, pageIndex, pageSize);

        var result = await sender.Send(query, cancellationToken);
        if (result.IsSuccess)
        {
            var mapped = mapper.Map<PagedListViewModel<LogEventViewModel>>(result.Value!);
            return Results.Ok(mapped);
        }
        else
        {
            return Results.BadRequest(result.Error);
        }
    }
}
