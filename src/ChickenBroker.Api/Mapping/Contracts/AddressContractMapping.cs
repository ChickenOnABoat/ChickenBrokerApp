using ChickenBroker.Application.Models;
using ChickenBroker.Contracts.Requests.Address;

namespace ChickenBroker.Api.Mapping.Contracts;

public static class AddressContractMapping
{
    public static Address MapToAddress(this UpdateAddressRequest request, Guid id)
    {
        return new Address
        {
            Id = id,
            City = request.City,
            Neighbourhood = request.Neighbourhood,
            Street = request.Street,
            State = request.State,
            StateAcronym = request.StateAcronym,
            ZipCode = request.ZipCode
        };
    }
}