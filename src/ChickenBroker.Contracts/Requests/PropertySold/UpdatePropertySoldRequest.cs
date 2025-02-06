namespace ChickenBroker.Contracts.Requests.PropertySold;

public class UpdatePropertySoldRequest
{
    public Guid? IdUserSold { get; set; }
    public required bool UserCreatorDidAgencyOfProperty { get; set; }
    public required bool UserCreatorDidSaleOfProperty { get; set; }
    
    public required Guid IdPropertyType {get; set; }
    public required string AddressNumber { get; set; }
    public required string AddressComplement { get; set; }
    public required string ZipCode { get; set; }
    public required int NumberOfBedrooms { get; set; }
    public required int NumberOfSuites { get; set; }
    public required int NumberOfBathrooms { get; set; }
    public required int NumberOfParkingSpots { get; set; }
    public required float PropertyArea { get; set; }
    public required int YearOfConstruction { get; set; }
    public required bool HasLobby { get; set; }
    public required DateTime DateOfSale { get; set; }
    public required string TimeElapsedToSell { get; set; }
    public required float AnnouncedValue { get; set; }
    public required float SaleValue { get; set; }
    public required float CommissionValue { get; set; }
}