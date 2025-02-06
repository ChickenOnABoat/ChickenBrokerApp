using ChickenBroker.Application.Models;
using ChickenBroker.Application.Models.Options;
using ChickenBroker.Contracts.Requests.PropertySold;
using ChickenBroker.Contracts.Responses;
using ChickenBroker.Contracts.Responses.PropertySold;

namespace ChickenBroker.Api.Mapping.Contracts;

public static class PropertySoldContractMapping
{
    public static PropertySold MapToPropertySold(this CreatePropertySoldRequest createPropertySoldRequest, Guid userId)
    {
        return new PropertySold
        {
            Id = Guid.NewGuid(),
            IdUserSold = createPropertySoldRequest.IdUserSold,
            IdUserCreator = userId,
            IdPropertyType = createPropertySoldRequest.IdPropertyType,
            UserCreatorDidAgencyOfProperty = createPropertySoldRequest.UserCreatorDidAgencyOfProperty,
            UserCreatorDidSaleOfProperty = createPropertySoldRequest.UserCreatorDidSaleOfProperty,
            AddressNumber = createPropertySoldRequest.AddressNumber,
            AddressComplement = createPropertySoldRequest.AddressComplement,
            NumberOfBedrooms = createPropertySoldRequest.NumberOfBedrooms,
            NumberOfSuites = createPropertySoldRequest.NumberOfSuites,
            NumberOfBathrooms = createPropertySoldRequest.NumberOfBathrooms,
            NumberOfParkingSpots = createPropertySoldRequest.NumberOfParkingSpots,
            PropertyArea = createPropertySoldRequest.PropertyArea,
            YearOfConstruction = createPropertySoldRequest.YearOfConstruction,
            HasLobby = createPropertySoldRequest.HasLobby,
            DateOfSale = createPropertySoldRequest.DateOfSale,
            TimeElapsedToSell = createPropertySoldRequest.TimeElapsedToSell,
            AnnouncedValue = createPropertySoldRequest.AnnouncedValue,
            SaleValue = createPropertySoldRequest.SaleValue,
            CommissionValue = createPropertySoldRequest.CommissionValue,
            IdAddress = default,
            ZipCode = createPropertySoldRequest.ZipCode,
        };
    }

    public static PropertySold MapToPropertySold(this UpdatePropertySoldRequest updatePropertySoldRequest, Guid id)
    {
        return new PropertySold
        {
            Id = id,
            IdUserSold = updatePropertySoldRequest.IdUserSold,
            IdUserCreator = Guid.Empty,
            IdPropertyType = updatePropertySoldRequest.IdPropertyType,
            UserCreatorDidAgencyOfProperty = updatePropertySoldRequest.UserCreatorDidAgencyOfProperty,
            UserCreatorDidSaleOfProperty = updatePropertySoldRequest.UserCreatorDidSaleOfProperty,
            AddressNumber = updatePropertySoldRequest.AddressNumber,
            AddressComplement = updatePropertySoldRequest.AddressComplement,
            NumberOfBedrooms = updatePropertySoldRequest.NumberOfBedrooms,
            NumberOfSuites = updatePropertySoldRequest.NumberOfSuites,
            NumberOfBathrooms = updatePropertySoldRequest.NumberOfBathrooms,
            NumberOfParkingSpots = updatePropertySoldRequest.NumberOfParkingSpots,
            PropertyArea = updatePropertySoldRequest.PropertyArea,
            YearOfConstruction = updatePropertySoldRequest.YearOfConstruction,
            HasLobby = updatePropertySoldRequest.HasLobby,
            DateOfSale = updatePropertySoldRequest.DateOfSale,
            TimeElapsedToSell = updatePropertySoldRequest.TimeElapsedToSell,
            AnnouncedValue = updatePropertySoldRequest.AnnouncedValue,
            SaleValue = updatePropertySoldRequest.SaleValue,
            CommissionValue = updatePropertySoldRequest.CommissionValue,
            IdAddress = default,
            ZipCode = updatePropertySoldRequest.ZipCode,
        };
    }

    public static PropertySoldResponse MapToResponse(this PropertySold propertySold)
    {
        return new PropertySoldResponse
        {
            Id = propertySold.Id,
            IdUserSold = propertySold.IdUserSold,
            IdPropertyType = propertySold.IdPropertyType,
            IdUserCreator = propertySold.IdUserCreator,
            UserCreatorDidAgencyOfProperty = propertySold.UserCreatorDidAgencyOfProperty,
            UserCreatorDidSaleOfProperty = propertySold.UserCreatorDidSaleOfProperty,
            AddressNumber = propertySold.AddressNumber,
            AddressComplement = propertySold.AddressComplement,
            NumberOfBedrooms = propertySold.NumberOfBedrooms,
            NumberOfSuites = propertySold.NumberOfSuites,
            NumberOfBathrooms = propertySold.NumberOfBathrooms,
            NumberOfParkingSpots = propertySold.NumberOfParkingSpots,
            PropertyArea = propertySold.PropertyArea,
            YearOfConstruction = propertySold.YearOfConstruction,
            HasLobby = propertySold.HasLobby,
            DateOfSale = propertySold.DateOfSale,
            TimeElapsedToSell = propertySold.TimeElapsedToSell,
            AnnouncedValue = propertySold.AnnouncedValue,
            SaleValue = propertySold.SaleValue,
            CommissionValue = propertySold.CommissionValue,
            Address = null,
            City = null,
            Neighbourhood = null,
        };
    }

    public static PagedResponse<PropertySoldResponse> MapToResponse(this IEnumerable<PropertySold> propertiesSolds, int page, int pageSize, int totalCount)
    {
        return new PagedResponse<PropertySoldResponse>
        {
            Items = propertiesSolds.Select(MapToResponse),
            PageSize = pageSize,
            Page = page,
            Total = totalCount
        };
    }

    public static GetAllPropertySoldOptions MapToOptions(this GetAllPropertySoldRequest request, Guid? userId)
    {
        return new GetAllPropertySoldOptions()
        {
            UserId = userId,
            ZipCode = request.ZipCode,
            AnnouncedValue = request.AnnouncedValue,
            AnnouncedValueGreaterOrEqual = request.AnnouncedValueGreaterOrEqualThan,
            AnnouncedValueLesserOrEqual = request.AnnouncedValueLesserOrEqualThan,
            City = request.City,
            SortField = request.SortBy?.Trim('+', '-'),
            SortOrder = request.SortBy is null ? FieldSortOrder.Unsorted :
                request.SortBy.StartsWith("-") ? FieldSortOrder.Descending : FieldSortOrder.Ascending,
        };
    }
    
}