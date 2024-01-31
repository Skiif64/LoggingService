using LoggingService.Domain.Shared;
using LoggingService.WebApi.Contracts.Common;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LoggingService.WebApi.Controllers;

[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    protected IMapper Mapper { get; }
    protected ISender Sender { get; }
    protected ILogger Logger { get; }

    public ApiBaseController(IMapper mapper, ISender sender, ILoggerFactory loggerFactory)
    {
        Mapper = mapper;
        Sender = sender;
        Logger = loggerFactory.CreateLogger(GetType());
    }

    protected IActionResult AsActionResult(Result result)
    {
        if (result.IsSuccess)
            return NoContent();

        return HandleErrorResult(result.Error);
    }

    protected IActionResult AsActionResult<TResult>(Result<TResult> result)
    {
        if (result.IsSuccess)
            return Ok(result.Value);

        return HandleErrorResult(result.Error);
    }

    protected IActionResult AsActionResult<TResult, TModel>(Result<TResult> result)
    {
        if (result.IsSuccess)
            return Ok(Mapper.Map<TModel>(result.Value!));

        return HandleErrorResult(result.Error);
    }

    private IActionResult HandleErrorResult(Error error)
    {
        var errorModel = Mapper.Map<ErrorViewModel>(error);
        return error.Type switch
        {
            ErrorType.Problem => BadRequest(errorModel),
            ErrorType.Validation => BadRequest(errorModel),
            ErrorType.Conflict => Conflict(errorModel),
            ErrorType.AccessDenied => Forbid(),
            ErrorType.NotFound => NotFound(errorModel),
            _ => throw new ArgumentException($"Cannot handle error with type: {error.Type}")
        };
    }
}