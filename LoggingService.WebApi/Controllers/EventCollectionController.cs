using LoggingService.Application.Features.EventCollections.Commands.Create;
using LoggingService.Application.Features.EventCollections.Commands.Queries;
using LoggingService.WebApi.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LoggingService.WebApi.Controllers;

[ApiController]
[Route("api/collection")]
public sealed class EventCollectionController : ControllerBase
{
    private readonly ISender _sender;

    public EventCollectionController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateEventCollectionViewModel model, CancellationToken cancellationToken)
    {
        var command = new CreateEventCollectionCommand(model.Name, Guid.NewGuid());

        var result = await _sender.Send(command, cancellationToken);
        if(result.IsSuccess)
        {
            return Ok();
        }
        else
        {
            return BadRequest(result.Error);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetPagedAsync(int pageIndex = 0, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var query = new GetPagedEventCollectionQuery(pageIndex, pageSize);

        var result = await _sender.Send(query, cancellationToken);
        if(result.IsSuccess)
        {
            return Ok(result.Value); //TODO: map
        }
        else
        {
            return BadRequest(result.Error);
        }
    }
}
