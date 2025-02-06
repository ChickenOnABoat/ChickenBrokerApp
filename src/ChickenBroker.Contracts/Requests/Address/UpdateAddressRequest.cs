namespace ChickenBroker.Contracts.Requests.Address;

public class UpdateAddressRequest
{
    public required string Street { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string StateAcronym { get; set; }
    public required string Neighbourhood { get; set; }
    public required string ZipCode { get; set; }
}