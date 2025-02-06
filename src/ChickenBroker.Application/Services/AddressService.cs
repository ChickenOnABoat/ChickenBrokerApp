using ChickenBroker.Application.Mapping;
using ChickenBroker.Application.Models;
using ChickenBroker.Application.Repositories.Interfaces;
using ChickenBroker.Application.Services.Interfaces;
using SDKBrasilAPI;
using SDKBrasilAPI.Responses;

namespace ChickenBroker.Application.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IBrasilAPI _brasilApi;

    public AddressService(IAddressRepository addressRepository, IBrasilAPI brasilApi)
    {
        _addressRepository = addressRepository;
        _brasilApi = brasilApi;
    }

    public async Task<Address?> CreateAsync(string zipCode, CancellationToken token = default)
    {

        var resultApi = await _brasilApi.CEP_V2(zipCode);
        if (resultApi is null)
        {
            throw new Exception("Address not found");
        }
        var stateData = await _brasilApi.IBGE_UF(resultApi.UF);
        if (stateData is null)
        {
            throw new Exception("UF not available on Address");
        }
        var address = resultApi.MapToAddress(Guid.NewGuid(), stateData);
        var result = await _addressRepository.CreateAsync(address, token);
        return result;
    }

    public Task<Address?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return _addressRepository.GetByIdAsync(id, token);
    }
    
    public async Task<Address?> GetByZipCodeAsync(string zipCode, CancellationToken token = default)
    {
        return await _addressRepository.GetByZipCodeAsync(zipCode, token);
    }

    public async Task<Address?> GetByZipCodeAndCreateIfNotExistsAsync(string zipCode, CancellationToken token = default)
    {
        var existsOnDb = await _addressRepository.ExistsByZipCodeAsync(zipCode, token);
        if (existsOnDb)
        {
            return await _addressRepository.GetByZipCodeAsync(zipCode, token);
        }
        
        return await CreateAsync(zipCode, token);
    }

    public Task<IEnumerable<Address>> GetAllAsync(CancellationToken token = default)
    {
        return _addressRepository.GetAllAsync(token);
    }

    public async Task<Address?> UpdateAsync(Address address, CancellationToken token = default)
    {
        var result =await _addressRepository.UpdateAsync(address, token);
        return  result;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _addressRepository.DeleteByIdAsync(id, token);
    }

    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        return _addressRepository.ExistsByIdAsync(id, token);
    }

    public Task<bool> ExistsByZipCodeAsync(string zipCode, CancellationToken token = default)
    {
        return _addressRepository.ExistsByZipCodeAsync(zipCode, token);
    }
}