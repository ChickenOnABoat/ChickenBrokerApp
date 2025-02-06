namespace ChickenBroker.Application.Models;

public class PropertyAgency
{
    public required Guid Id { get; init; }
    public required string Name { get; set; }
    public required string IdentificationDocumentNumber  { get; set; }
    public Guid IdAddress  { get; set; }
    public string? AddressNumber  { get; set; }
    public string? AddressComplement  { get; set; }
    public required string ZipCode  { get; set; }
    public string? ContactNumber  { get; set; }
    public Address? Address { get; set; }
}