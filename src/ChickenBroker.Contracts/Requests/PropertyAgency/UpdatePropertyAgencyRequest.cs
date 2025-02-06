namespace ChickenBroker.Contracts.Requests.PropertyAgency;

public class UpdatePropertyAgencyRequest
{
    public required string Name { get; set; }
    public required string IdentificationDocumentNumber  { get; set; }
    public string? AddressNumber  { get; set; }
    public string? AddressComplement  { get; set; }
    public required string ZipCode  { get; set; }
    public string? ContactNumber  { get; set; }
    
}