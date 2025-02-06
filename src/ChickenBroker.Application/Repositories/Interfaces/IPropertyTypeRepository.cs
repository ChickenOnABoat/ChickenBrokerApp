using ChickenBroker.Application.Models;

namespace ChickenBroker.Application.Repositories.Interfaces;

public interface IPropertyTypeRepository
{
    Task<IEnumerable<PropertyType>> GetAllAsync(CancellationToken token = default);
}