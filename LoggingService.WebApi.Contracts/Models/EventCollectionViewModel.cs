namespace LoggingService.WebApi.Contracts.Models;
public sealed class EventCollectionViewModel
{
    public string Name { get; set; } = null!;
    public Guid? ApplicationId { get; set; }
}
