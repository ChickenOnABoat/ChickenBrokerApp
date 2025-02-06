using ChickenBroker.Application.Models;
using ChickenBroker.Application.Repositories;
using ChickenBroker.Application.Repositories.Interfaces;
using FluentValidation;
using SDKBrasilAPI;

namespace ChickenBroker.Application.Validators;

public class PropertySoldValidator : AbstractValidator<PropertySold>
{
    private readonly IUserRepository _userRepository;
    private readonly IPropertySoldRepository _propertySoldRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IBrasilAPI _brasilApi;
    
    public PropertySoldValidator(IUserRepository userRepository, IPropertySoldRepository propertySoldRepository, IAddressRepository addressRepository, IBrasilAPI brasilApi)
    {
        _userRepository = userRepository;
        _propertySoldRepository = propertySoldRepository;
        _addressRepository = addressRepository;
        _brasilApi = brasilApi;

        RuleSet("Create", () =>
        {
            RuleFor(x => x.IdUserCreator).NotEmpty().WithMessage("Creator Id cannot be empty");
            RuleFor(x => x.UserCreatorDidAgencyOfProperty).NotEmpty().WithMessage("UserCreatorDidAgencyOfProperty cannot be empty");
            RuleFor(x => x.UserCreatorDidSaleOfProperty).NotEmpty().WithMessage("UserCreatorDidSaleOfProperty cannot be empty");
            RuleFor(x => x.IdPropertyType).NotEmpty().WithMessage("IdPropertyType cannot be empty");
            
            RuleFor(x => x.NumberOfBedrooms).NotNull().WithMessage("NumberOfBedrooms cannot be empty");
            RuleFor(x => x.NumberOfSuites).NotNull().WithMessage("NumberOfSuites cannot be empty");
            RuleFor(x => x.NumberOfBathrooms).NotNull().WithMessage("NumberOfBathrooms cannot be empty");
            RuleFor(x => x.NumberOfParkingSpots).NotNull().WithMessage("NumberOfParkingSpots cannot be empty");
            
            RuleFor(x => x.PropertyArea).NotNull().WithMessage("PropertyArea cannot be empty");
            RuleFor(x => x.YearOfConstruction).NotNull().WithMessage("YearOfConstruction cannot be empty");
            RuleFor(x => x.YearOfConstruction).LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("YearOfConstruction cannot be greater than current year");
            
            RuleFor(x => x.HasLobby).NotNull().WithMessage("HasLobby cannot be empty");
            RuleFor(x => x.DateOfSale).NotNull().WithMessage("DateOfSale cannot be empty");
            RuleFor(x => x.TimeElapsedToSell).NotNull().WithMessage("TimeElapsedToSell cannot be empty");
            
            RuleFor(x => x.AnnouncedValue).NotNull().WithMessage("AnnouncedValue cannot be empty");
            RuleFor(x => x.SaleValue).NotNull().WithMessage("SaleValue cannot be empty");
            RuleFor(x => x.CommissionValue).NotNull().WithMessage("CommissionValue cannot be empty");
            
            RuleFor(x => x.IdUserCreator).MustAsync(ValidateUserExists).WithMessage("IdUserCreator does not exist");
            RuleFor(x => x.IdUserSold).MustAsync(ValidateUserSoldExists).WithMessage("IdUserSold does not exist");
            RuleFor(x => x.ZipCode).Cascade(CascadeMode.Stop).Length(8).WithMessage("ZipCode must be 8 digits")
                .MustAsync(IsValidZipCode).WithMessage("ZipCode is not valid");
        });
        
        RuleSet("Update", () =>
        {
            RuleFor(x => x.UserCreatorDidAgencyOfProperty).NotEmpty().WithMessage("UserCreatorDidAgencyOfProperty cannot be empty");
            RuleFor(x => x.UserCreatorDidSaleOfProperty).NotEmpty().WithMessage("UserCreatorDidSaleOfProperty cannot be empty");
            RuleFor(x => x.IdPropertyType).NotEmpty().WithMessage("IdPropertyType cannot be empty");
            
            RuleFor(x => x.NumberOfBedrooms).NotNull().WithMessage("NumberOfBedrooms cannot be empty");
            RuleFor(x => x.NumberOfSuites).NotNull().WithMessage("NumberOfSuites cannot be empty");
            RuleFor(x => x.NumberOfBathrooms).NotNull().WithMessage("NumberOfBathrooms cannot be empty");
            RuleFor(x => x.NumberOfParkingSpots).NotNull().WithMessage("NumberOfParkingSpots cannot be empty");
            
            RuleFor(x => x.PropertyArea).NotNull().WithMessage("PropertyArea cannot be empty");
            RuleFor(x => x.YearOfConstruction).NotNull().WithMessage("YearOfConstruction cannot be empty");
            RuleFor(x => x.YearOfConstruction).LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("YearOfConstruction cannot be greater than current year");
            
            RuleFor(x => x.HasLobby).NotNull().WithMessage("HasLobby cannot be empty");
            RuleFor(x => x.DateOfSale).NotNull().WithMessage("DateOfSale cannot be empty");
            RuleFor(x => x.TimeElapsedToSell).NotNull().WithMessage("TimeElapsedToSell cannot be empty");
            
            RuleFor(x => x.AnnouncedValue).NotNull().WithMessage("AnnouncedValue cannot be empty");
            RuleFor(x => x.SaleValue).NotNull().WithMessage("SaleValue cannot be empty");
            RuleFor(x => x.CommissionValue).NotNull().WithMessage("CommissionValue cannot be empty");
            RuleFor(x => x.ZipCode).Cascade(CascadeMode.Stop).Length(8).WithMessage("ZipCode must be 8 digits")
                .MustAsync(IsValidZipCode).WithMessage("ZipCode is not valid");
            RuleFor(x => x.Id).CustomAsync(async (x, context, cancellation) =>
            {
                if(context.RootContextData.TryGetValue("currentUserId", out var value))
                {
                    Guid currentUserId = Guid.Parse(value.ToString() ?? throw new InvalidOperationException());
                    var resultCheck = await ValidateUserCreatorIsCurrentUser(x, currentUserId);
                    if (!resultCheck)
                    {
                        context.AddFailure("User does not match authenticated user.");
                    }
                }
                else
                {
                    context.AddFailure("User is not present to create object");
                }
            });
            
        });

    }
    
    private async Task<bool> ValidateUserExists(PropertySold propertySold, Guid id,CancellationToken token = default)
    {
        var existingUser = await _userRepository.ExistsByIdAsync(id, token);
        return existingUser;
    }
    
    private async Task<bool> ValidateUserSoldExists(PropertySold propertySold, Guid? id,CancellationToken token = default)
    {
        if (id is null)
        {
            return true;
        }
        var existingUser = await _userRepository.ExistsByIdAsync((Guid)id, token);
        return existingUser;
    }

    private async Task<bool> ValidateUserCreatorIsCurrentUser(Guid id,
        Guid currentUserId, CancellationToken token = default)
    {
        return await _propertySoldRepository.PropertySoldMatchCreatorAndCurrentIdAsync(id, currentUserId, token);
    }
    
    private async Task<bool> IsValidZipCode(PropertySold propertySold,string? zipCode, CancellationToken cancellationToken = default)
    {
        if (zipCode is null || zipCode.Length != 8)
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
            var resultApi = await _brasilApi.CEP_V2(zipCode);
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