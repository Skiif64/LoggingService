using LoggingService.Application.Features.LogEvents.Commands;
using LoggingService.Application.Features.LogEvents.Commands.Create;
using LoggingService.Application.Features.LogEvents.Commands.CreateBatched;
using LoggingService.Application.Features.LogEvents.Queries.GetPaged;
using LoggingService.WebApi.Contracts.Common;
using LoggingService.WebApi.Contracts.Models;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LoggingService.WebApi.Controllers;

[ApiController]
[Route("api/event")]
public sealed class LogEventController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public LogEventController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPost("{collectionName}")]
    public async Task<IActionResult> InsertAsync(string collectionName, CreateLogEventViewModel model, CancellationToken cancellationToken)
    {
        var command = new CreateLogEventCommand(collectionName, _mapper.Map<CreateLogEventDto>(model));
        var result = await _sender.Send(command, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok();
        }
        else
        {
            return BadRequest(result.Error);
        }
    }

    [HttpPost("{collectionName}/batched")]
    public async Task<IActionResult> InsertBatchedAsync(string collectionName, IEnumerable<CreateLogEventViewModel> models, CancellationToken cancellationToken)
    {
        var command = new CreateLogEventBatchedCommand(collectionName,
            _mapper.Map<IEnumerable<CreateLogEventDto>>(models));
        var result = await _sender.Send(command, cancellationToken);
        if (result.IsSuccess)
        {
            return Ok();
        }
        else
        {
            return BadRequest(result.Error);
        }
    }

    [HttpGet("{collectionName}")]
    public async Task<IActionResult> GetPagedAsync(string collectionName, int pageIndex = 0, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        //TODO: validate page index & size
        var query = new GetPagedLogEventsQuery(collectionName, pageIndex, pageSize);

        var result = await _sender.Send(query, cancellationToken);
        if (result.IsSuccess)
        {
            var mapped = _mapper.Map<PagedListViewModel<LogEventViewModel>>(result.Value!);
            return Ok(mapped);
        }
        else
        {
            return BadRequest(result.Error);
        }
    }
}
