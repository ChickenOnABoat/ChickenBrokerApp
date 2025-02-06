namespace ChickenBroker.Contracts.Requests;

public interface IPagedRequest
{
    public int Page { get; init; }
    public int PageSize { get; init; }
}