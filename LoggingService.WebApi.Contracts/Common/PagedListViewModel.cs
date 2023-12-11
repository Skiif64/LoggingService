namespace LoggingService.WebApi.Contracts.Common;
public sealed class PagedListViewModel<TItem>
{
    public required IReadOnlyCollection<TItem> Items { get; init; }
    public required int CurrentCount { get; init; }
    public required int PageIndex { get; init; }
    public required int PageSize { get; init; }
    public required int TotalPages { get; init; }
    public required int TotalCount { get; init; }
    public required bool HasNextPage { get; init; }
    public required bool HasPrevPage { get; init; }
}
