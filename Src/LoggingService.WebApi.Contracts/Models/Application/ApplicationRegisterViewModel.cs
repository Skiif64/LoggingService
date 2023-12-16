namespace LoggingService.WebApi.Contracts.Models.Application;
public sealed class ApplicationRegisterViewModel
{
    public required string Name { get; init; }
    public required DateTime ExpireAtUtc { get; init; }
}
