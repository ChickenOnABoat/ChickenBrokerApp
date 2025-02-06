using ChickenBroker.Application.Models;
using ChickenBroker.Application.Models.Options;
using ChickenBroker.Application.Repositories.Interfaces;
using ChickenBroker.Application.Services.Interfaces;
using FluentValidation;

namespace ChickenBroker.Application.Services;

public class PropertyAgencyService : IPropertyAgencyService
{
    private readonly IPropertyAgencyRepository _propertyAgencyRepository;
    private readonly IValidator<PropertyAgency> _propertyAgencyValidator;
    private readonly IAddressService _addressService;
    private readonly IValidator<GetAllPropertyAgencyOptions> _getAllPropertyAgencyOptionsValidator;

    public PropertyAgencyService(IPropertyAgencyRepository propertyAgencyRepository, IValidator<PropertyAgency> propertyAgencyValidator, IAddressService addressService, IValidator<GetAllPropertyAgencyOptions> getAllPropertyAgencyOptionsValidator)
    {
        _propertyAgencyRepository = propertyAgencyRepository;
        _propertyAgencyValidator = propertyAgencyValidator;
        _addressService = addressService;
        _getAllPropertyAgencyOptionsValidator = getAllPropertyAgencyOptionsValidator;
    }


    public async Task<PropertyAgency?> CreateAsync(PropertyAgency propertyAgency, CancellationToken token = default)
    {
        await _propertyAgencyValidator.ValidateAndThrowAsync(propertyAgency, token);
        var address = await _addressService.GetByZipCodeAndCreateIfNotExistsAsync(propertyAgency.ZipCode, token);
        if (address is null)
        {
            return null;
        }
        propertyAgency.IdAddress = address.Id;
        return await _propertyAgencyRepository.CreateAsync(propertyAgency, token);
    }

    public Task<PropertyAgency?> GetByIdAsync(Guid id, Guid? userid = default, CancellationToken token = default)
    {
        return _propertyAgencyRepository.GetByIdAsync(id, userid, token);
    }

    public async Task<IEnumerable<PropertyAgency>> GetAllAsync(GetAllPropertyAgencyOptions options, CancellationToken token = default)
    {
        await _getAllPropertyAgencyOptionsValidator.ValidateAndThrowAsync(options, token);
        return await _propertyAgencyRepository.GetAllAsync(options,token);
    }

    public Task<int> GetCountAllAsync(string? name, string? city, string? identificationDocumentNumber, string? zipCode,
        CancellationToken token = default)
    {
        return _propertyAgencyRepository.GetCountAllAsync(name, city, identificationDocumentNumber, zipCode, token);
    }

    public async Task<PropertyAgency?> UpdateAsync(PropertyAgency propertyAgency, CancellationToken token = default)
    {
        await _propertyAgencyValidator.ValidateAndThrowAsync(propertyAgency, token);
        var address = await _addressService.GetByZipCodeAndCreateIfNotExistsAsync(propertyAgency.ZipCode, token);
        if (address is null)
        {
            return null;
        }
        propertyAgency.IdAddress = address.Id;
        return await _propertyAgencyRepository.UpdateAsync(propertyAgency, token);
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _propertyAgencyRepository.DeleteByIdAsync(id, token);
    }

    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        return _propertyAgencyRepository.ExistsByIdAsync(id, token);
    }
}