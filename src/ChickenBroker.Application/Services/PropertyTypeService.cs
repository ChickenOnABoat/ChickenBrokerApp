using ChickenBroker.Application.Models;
using ChickenBroker.Application.Repositories.Interfaces;
using ChickenBroker.Application.Services.Interfaces;

namespace ChickenBroker.Application.Services;

public class PropertyTypeService : IPropertyTypeService
{
    private readonly IPropertyTypeRepository _propertyTypeRepository;

    public PropertyTypeService(IPropertyTypeRepository propertyTypeRepository)
    {
        _propertyTypeRepository = propertyTypeRepository;
    }

    public Task<IEnumerable<PropertyType>> GetAllAsync(CancellationToken token = default)
    {
        return _propertyTypeRepository.GetAllAsync(token);
    }
}