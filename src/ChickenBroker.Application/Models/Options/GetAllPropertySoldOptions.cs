namespace ChickenBroker.Application.Models.Options;

public class GetAllPropertySoldOptions
{
    
    public string? ZipCode { get; set; }
    
    public string? City { get; set; }
    
    public float? AnnouncedValue { get; set; }
    public float? AnnouncedValueGreaterOrEqual { get; set; }
    public float? AnnouncedValueLesserOrEqual { get; set; }
    public Guid? UserId { get; set; }
    
    public string? SortField { get; set; }
    
    
    public FieldSortOrder? SortOrder { get; set; }
}