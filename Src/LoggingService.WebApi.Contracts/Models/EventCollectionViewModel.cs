namespace LoggingService.WebApi.Contracts.Models;
public sealed class EventCollectionViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid? ApplicationId { get; set; }
}
