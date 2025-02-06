using ChickenBroker.Application.Models;
using ChickenBroker.Application.Models.Options;
using ChickenBroker.Application.Repositories.Interfaces;
using ChickenBroker.Application.Services.Interfaces;
using FluentValidation;

namespace ChickenBroker.Application.Services;

public class PropertySoldService :  IPropertySoldService
{

    private readonly IPropertySoldRepository _propertySoldRepository;
    private readonly IValidator<PropertySold> _propertySoldValidator;
    private readonly IAddressService _addressService;

    public PropertySoldService(IPropertySoldRepository propertySoldRepository, IValidator<PropertySold> propertySoldValidator, IAddressService addressService)
    {
        _propertySoldRepository = propertySoldRepository;
        _propertySoldValidator = propertySoldValidator;
        _addressService = addressService;
    }

    public async Task<PropertySold?> CreateAsync(PropertySold propertySold, Guid userId, CancellationToken token = default)
    {
        var contextValidation = ValidationContext<PropertySold>.CreateWithOptions(propertySold, options => options.IncludeRuleSets("Create"));
        contextValidation.RootContextData["currentUserId"] = userId; 
        var validationResults = await _propertySoldValidator.ValidateAsync(contextValidation, token);
        if (!validationResults.IsValid)
        {
            var ex = new ValidationException(validationResults.Errors);
            throw new ArgumentException(ex.Message, ex);
        }
        var address = await _addressService.GetByZipCodeAndCreateIfNotExistsAsync(propertySold.ZipCode, token);
        if (address is null) return null;
        propertySold.IdAddress = address.Id;
        
        return await _propertySoldRepository.CreateAsync(propertySold, token);
    }

    public Task<PropertySold?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
    {
        return _propertySoldRepository.GetByIdAsync(id, userId, token);
    }

    public Task<IEnumerable<PropertySold>> GetAllAsync(GetAllPropertySoldOptions options, CancellationToken token = default)
    {
        return _propertySoldRepository.GetAllAsync(options, token);
    }

    public async Task<PropertySold?> UpdateAsync(PropertySold propertySold, Guid userId, CancellationToken token = default)
    {
        //Validate if ID UserCreator is same that is updating
        var contextValidation = ValidationContext<PropertySold>.CreateWithOptions(propertySold, options => options.IncludeRuleSets("Update"));
        contextValidation.RootContextData["currentUserId"] = userId; 
        var validationResults = await _propertySoldValidator.ValidateAsync(contextValidation , token);
        if (!validationResults.IsValid)
        {
            throw new ValidationException(validationResults.Errors);
        }
        var address = await _addressService.GetByZipCodeAndCreateIfNotExistsAsync(propertySold.ZipCode, token);
        if (address != null) propertySold.IdAddress = address.Id;
        return await _propertySoldRepository.UpdateAsync(propertySold, token);
    }

    public Task<bool> DeleteByIdAsync(Guid id, Guid userId, CancellationToken token = default)
    {
        return _propertySoldRepository.DeleteByIdAsync(id, userId, token);
    }

    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        return _propertySoldRepository.ExistsByIdAsync(id, token);
    }
}