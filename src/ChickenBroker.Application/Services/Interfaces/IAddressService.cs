using ChickenBroker.Application.Models;

namespace ChickenBroker.Application.Services.Interfaces;

public interface IAddressService
{
    Task<Address?> CreateAsync(string zipCode, CancellationToken token = default);
    
    Task<Address?> GetByIdAsync(Guid id, CancellationToken token = default);
    
    Task<Address?> GetByZipCodeAsync(string zipCode, CancellationToken token = default);
    Task<Address?> GetByZipCodeAndCreateIfNotExistsAsync(string zipCode, CancellationToken token = default);
    
    Task<IEnumerable<Address>> GetAllAsync(CancellationToken token = default);
    
    Task<Address?> UpdateAsync(Address address, CancellationToken token = default);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
    
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
    
    Task<bool> ExistsByZipCodeAsync(string zipCode, CancellationToken token = default);
}