namespace LoggingService.WebApi.Contracts.Models.Application;
public sealed class ApplicationRenewViewModel
{
    public required Guid ApplicationId { get; init; }
    public required DateTime ExpireAtUtc { get; init; }
}
