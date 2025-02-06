using ChickenBroker.Application.Models;
using ChickenBroker.Application.Repositories.Interfaces;
using FluentValidation;
using SDKBrasilAPI;

namespace ChickenBroker.Application.Validators;

public class PropertyAgencyValidator : AbstractValidator<PropertyAgency>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IBrasilAPI _brasilAPI;
    
    public PropertyAgencyValidator(IAddressRepository addressRepository, IBrasilAPI brasilApi)
    {
        _addressRepository = addressRepository;
        _brasilAPI = brasilApi;
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
        RuleFor(x => x.IdentificationDocumentNumber).NotEmpty().WithMessage("IdentificationDocumentNumber cannot be empty");
        RuleFor(x => x.ZipCode).NotEmpty().WithMessage("ZipCode cannot be empty");
        RuleFor(x => x.ZipCode).Cascade(CascadeMode.Stop).Length(8).WithMessage("ZipCode must be 8 digits")
                                                .MustAsync(IsValidZipCode).WithMessage("ZipCode is not valid");
        
        
    }

    private async Task<bool> IsValidZipCode(PropertyAgency propertyAgency,string zipCode, CancellationToken cancellationToken = default)
    {
        if (zipCode.Length != 8)
        {
            return false;
        }
        
        var resultBd = await _addressRepository.ExistsByZipCodeAsync(zipCode, cancellationToken);
        if (resultBd)
        {
            return true;
        }

        try
        {
            var resultApi = await _brasilAPI.CEP_V2(zipCode);
            if (resultApi is not null)
            {
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
        
    }
}