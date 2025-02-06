using ChickenBroker.Application.Models;

namespace ChickenBroker.Application.Services.Interfaces;

public interface IPropertyTypeService
{
    Task<IEnumerable<PropertyType>> GetAllAsync(CancellationToken token = default);
}