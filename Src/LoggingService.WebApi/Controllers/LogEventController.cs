using LoggingService.Application.Authentication;
using LoggingService.Application.Features.LogEvents.Commands;
using LoggingService.Application.Features.LogEvents.Commands.Create;
using LoggingService.Application.Features.LogEvents.Commands.CreateBatched;
using LoggingService.Application.Features.LogEvents.Queries.GetPaged;
using LoggingService.Domain.Features.LogEvents;
using LoggingService.Domain.Shared;
using LoggingService.WebApi.Contracts.Common;
using LoggingService.WebApi.Contracts.Models;
using LoggingService.WebApi.Contracts.Requests;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoggingService.WebApi.Controllers;

[Route("api/events")]
public sealed class LogEventController : ApiBaseController
{
    public LogEventController(IMapper mapper, ISender sender, ILoggerFactory loggerFactory) 
        : base(mapper, sender, loggerFactory)
    {
    }
    
    [HttpPost]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.ApiKeyScheme)]
    public async Task<IActionResult> InsertAsync(CreateLogEventRequest model, CancellationToken cancellationToken)
    {
        var command = new CreateLogEventCommand(model.CollectionId, Mapper.Map<CreateLogEventDto>(model.LogModel));
        var result = await Sender.Send(command, cancellationToken);
        return AsActionResult(result);
    }
    
    [HttpPost("batch")]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.ApiKeyScheme)]
    public async Task<IActionResult> InsertBatchedAsync(CreateBatchedLogEventRequest model, CancellationToken cancellationToken)
    {
        var command = new CreateLogEventBatchedCommand(model.CollectionId,
            Mapper.Map<IEnumerable<CreateLogEventDto>>(model.LogModels));
        var result = await Sender.Send(command, cancellationToken);
        return AsActionResult(result);
    }

    [HttpGet("collection/{collectionId:guid}")]
    public async Task<IActionResult> GetPagedFromCollectionAsync(Guid collectionId, int pageSize, int pageIndex,
        CancellationToken cancellationToken)
    {
        //TODO: validate page index & size
        var query = new GetPagedLogEventsQuery(collectionId, pageIndex, pageSize);
        var result = await Sender.Send(query, cancellationToken);
        return AsActionResult<PagedList<LogEvent>, PagedListViewModel<LogEventViewModel>>(result);
    }
}