namespace ChickenBroker.Contracts.Requests.PropertySold;

public class GetAllPropertySoldRequest
{
    public string? ZipCode { get; set; }
    public string? City { get; set; }
    public float? AnnouncedValue { get; set; }
    public float? AnnouncedValueGreaterOrEqualThan { get; set; }
    public float? AnnouncedValueLesserOrEqualThan { get; set; }
    public string? SortBy { get; set; }
}