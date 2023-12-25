namespace LoggingService.WebApi.Contracts.Models.Application;
public sealed class ApplicationRenewViewModel
{
    public required string ApiKeyPrefix { get; init; }
    public required DateTime ExpireAtUtc { get; init; }
}
