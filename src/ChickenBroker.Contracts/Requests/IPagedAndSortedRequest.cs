namespace ChickenBroker.Contracts.Requests;

public interface IPagedAndSortedRequest
{
    
    public string SortBy { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}