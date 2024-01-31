using LoggingService.Application.Authentication;
using LoggingService.Application.Features.EventCollections;
using LoggingService.Application.Features.EventCollections.Commands.Create;
using LoggingService.Application.Features.EventCollections.Commands.Queries;
using LoggingService.Domain.Shared;
using LoggingService.WebApi.Contracts.Common;
using LoggingService.WebApi.Contracts.Models;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoggingService.WebApi.Controllers;

[Route("api/collections")]
public sealed class CollectionController : ApiBaseController
{
    public CollectionController(IMapper mapper, ISender sender, ILoggerFactory loggerFactory) 
        : base(mapper, sender, loggerFactory)
    {
    }
    
    [HttpPost]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.ApiKeyScheme)]
    public async Task<IActionResult> CreateAsync(CreateEventCollectionViewModel model, CancellationToken cancellationToken)
    {
        var command = new CreateEventCollectionCommand(model.Name, Guid.NewGuid()); //TODO: get app id

        var result = await Sender.Send(command, cancellationToken);
        return AsActionResult(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPagedAsync(int pageIndex = 0, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var query = new GetPagedEventCollectionQuery(pageIndex, pageSize);

        var result = await Sender.Send(query, cancellationToken);
        return AsActionResult<PagedList<EventCollectionDto>, PagedListViewModel<EventCollectionViewModel>>(result);
    }
}