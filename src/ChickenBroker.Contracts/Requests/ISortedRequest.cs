namespace ChickenBroker.Contracts.Requests;

public interface ISortedRequest
{
    public string? SortBy { get; init; }
}