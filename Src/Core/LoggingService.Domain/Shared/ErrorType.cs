namespace LoggingService.Domain.Shared;

public enum ErrorType
{
    Problem,
    Validation,
    NotFound,
    Conflict,
    AccessDenied,
}