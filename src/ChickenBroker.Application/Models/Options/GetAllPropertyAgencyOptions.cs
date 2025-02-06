namespace ChickenBroker.Application.Models.Options;

public class GetAllPropertyAgencyOptions
{
    public string? Name { get; set; }
    public string? IdentificationDocumentNumber { get; set; }
    public string? ZipCode { get; set; }
    public string? City { get; set; }
    
    public string? SortField { get; set; }
    
    public FieldSortOrder? SortOrder { get; set; }
    public int PageSize { get; set; }
    public int Page { get; set; }
}