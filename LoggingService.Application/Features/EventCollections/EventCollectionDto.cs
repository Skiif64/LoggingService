namespace LoggingService.Application.Features.EventCollections;
public sealed class EventCollectionDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
