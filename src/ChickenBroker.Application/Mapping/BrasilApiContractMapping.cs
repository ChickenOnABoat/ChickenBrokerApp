using ChickenBroker.Application.Models;
using SDKBrasilAPI.Responses;

namespace ChickenBroker.Application.Mapping;

public static class BrasilApiContractMapping
{
    public static Address MapToAddress(this CEPResponse cepResponse, Guid id, IBGEResponse stateData)
    {
        
        return new Address
        {
            Id = id,
            City = cepResponse.City,
            Neighbourhood = cepResponse.Neighborhood,
            State = stateData.IBGEs.FirstOrDefault()?.Nome ?? string.Empty,
            StateAcronym = stateData.IBGEs.FirstOrDefault()?.Sigla ?? string.Empty,
            Street = cepResponse.Street,
            ZipCode = cepResponse.CEP
        };
    }
}