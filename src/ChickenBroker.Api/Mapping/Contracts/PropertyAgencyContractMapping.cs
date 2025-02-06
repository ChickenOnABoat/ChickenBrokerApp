using System.Collections;
using ChickenBroker.Application.Models;
using ChickenBroker.Application.Models.Options;
using ChickenBroker.Contracts.Requests.PropertyAgency;
using ChickenBroker.Contracts.Responses.PropertyAgency;

namespace ChickenBroker.Api.Mapping.Contracts;

public static class PropertyAgencyContractMapping
{
    public static PropertyAgency MapToPropertyAgency(this CreatePropertyAgencyRequest request)
    {
        return new PropertyAgency
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            AddressNumber = request.AddressNumber,
            ContactNumber = request.ContactNumber,
            IdentificationDocumentNumber = request.IdentificationDocumentNumber,
            ZipCode = request.ZipCode
        };
    }
    
    public static PropertyAgency MapToPropertyAgency(this UpdatePropertyAgencyRequest request, Guid id)
    {
        return new PropertyAgency
        {
            Id = id,
            Name = request.Name,
            AddressNumber = request.AddressNumber,
            ContactNumber = request.ContactNumber,
            IdentificationDocumentNumber = request.IdentificationDocumentNumber,
            ZipCode = request.ZipCode
        };
    }

    public static PropertyAgencyResponse MapToResponse(this PropertyAgency propertyAgency)
    {
        
        return new PropertyAgencyResponse
        {
            Id = propertyAgency.Id,
            AddressNumber = propertyAgency.AddressNumber,
            ContactNumber = propertyAgency.ContactNumber,
            IdentificationDocumentNumber = propertyAgency.IdentificationDocumentNumber,
            Name = propertyAgency.Name,
            ZipCode = propertyAgency?.Address?.ZipCode ?? "00000000",
            IdAddress = (Guid)propertyAgency?.IdAddress!,
            Address = propertyAgency?.Address
        };
    }
    
    public static PropertyAgenciesResponse MapToResponse(this IEnumerable<PropertyAgency> propertyAgencies, int page, int pageSize, int totalCount)
    {
        return new PropertyAgenciesResponse
        {
            Items = propertyAgencies.Select(MapToResponse),
            Page = page,
            PageSize = pageSize,
            Total = totalCount
        };
    }

    public static GetAllPropertyAgencyOptions MapToOptions(this GetAllPropertyAgencyRequest request)
    {
        return new GetAllPropertyAgencyOptions
        {
            Name = request.Name,
            City = request.City,
            IdentificationDocumentNumber = request.IdentificationDocumentNumber,
            ZipCode = request.ZipCode,
            SortField = request.SortBy?.Trim('+','-'),
            SortOrder = request.SortBy is null ? FieldSortOrder.Unsorted : request.SortBy.StartsWith("-") ? FieldSortOrder.Descending : FieldSortOrder.Ascending,
            Page = request.Page,
            PageSize = request.PageSize,
        };
    }
}