namespace ChickenBroker.Contracts.Requests.PropertyAgency;

public class GetAllPropertyAgencyRequest : IPagedRequest, ISortedRequest
{
    public required string? Name { get; init; }
    public required string? IdentificationDocumentNumber { get; init; }
    public required string? ZipCode { get; init; }
    public required string? City { get; init; }
    public required string? SortBy { get; init; }
    public required int Page { get; init; } = 1;
    public required int PageSize { get; init; } = 10;
}