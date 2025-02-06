using ChickenBroker.Application.Models;
using ChickenBroker.Application.Models.Options;
using ChickenBroker.Application.Repositories.Interfaces;
using ChickenBroker.Application.Services;
using ChickenBroker.Application.Services.Interfaces;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;

namespace ChickenBroker.Application.Tests.Unit;

public class PropertySoldServiceTests
{
    private readonly  PropertySoldService _sut;
    private readonly IPropertySoldRepository _propertySoldRepository = Substitute.For<IPropertySoldRepository>();
    private readonly  IValidator<PropertySold> _propertySoldValidator = Substitute.For<IValidator<PropertySold>>();
    private readonly  IAddressService _addressService = Substitute.For<IAddressService>();
    
    public PropertySoldServiceTests()
    {
        _sut = new PropertySoldService(_propertySoldRepository, _propertySoldValidator, _addressService);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnPropertySold_WhenPropertySoldIsCreated()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var address = new Address()
        {
            City = "City",
            Id = Guid.NewGuid(),
            Neighbourhood = "Neighbourhood",
            State = "State",
            StateAcronym = "St",
            Street = "Street",
            ZipCode = "59000000"
        };
        var propertySold = new PropertySold
        {
            Id = Guid.NewGuid(),
            IdPropertyType = Guid.NewGuid(),
            ZipCode = "59000000",
            IdUserSold = userId,
            IdUserCreator = userId,
            UserCreatorDidAgencyOfProperty = true,
            UserCreatorDidSaleOfProperty = true,
            IdAddress = address.Id,
            AddressNumber = "",
            AddressComplement = "",
            NumberOfBedrooms = 0,
            NumberOfSuites = 0,
            NumberOfBathrooms = 0,
            NumberOfParkingSpots = 0,
            PropertyArea = 0,
            YearOfConstruction = 0,
            HasLobby = true,
            DateOfSale = DateTime.Today,
            TimeElapsedToSell = "",
            AnnouncedValue = 10,
            SaleValue = 10,
            CommissionValue = 1
        };
        _propertySoldValidator.ValidateAsync(Arg.Any<ValidationContext<PropertySold>>()).Returns(new ValidationResult()
        {
            Errors = new List<ValidationFailure>()
        });
        _addressService.GetByZipCodeAndCreateIfNotExistsAsync(propertySold.ZipCode).Returns(address);
        _propertySoldRepository.CreateAsync(propertySold).Returns(propertySold);
        //Act
        var result = await _sut.CreateAsync(propertySold, userId);
        //Assert
        result.Should().BeEquivalentTo(propertySold);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnNull_WhenPropertySoldIsNotCreated()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var address = new Address()
        {
            City = "City",
            Id = Guid.NewGuid(),
            Neighbourhood = "Neighbourhood",
            State = "State",
            StateAcronym = "St",
            Street = "Street",
            ZipCode = "59000000"
        };
        var propertySold = new PropertySold
        {
            Id = Guid.NewGuid(),
            IdPropertyType = Guid.NewGuid(),
            ZipCode = "59000000",
            IdUserSold = userId,
            IdUserCreator = userId,
            UserCreatorDidAgencyOfProperty = true,
            UserCreatorDidSaleOfProperty = true,
            IdAddress = address.Id,
            AddressNumber = "",
            AddressComplement = "",
            NumberOfBedrooms = 0,
            NumberOfSuites = 0,
            NumberOfBathrooms = 0,
            NumberOfParkingSpots = 0,
            PropertyArea = 0,
            YearOfConstruction = 0,
            HasLobby = true,
            DateOfSale = DateTime.Today,
            TimeElapsedToSell = "",
            AnnouncedValue = 10,
            SaleValue = 10,
            CommissionValue = 1
        };
        _propertySoldValidator.ValidateAsync(Arg.Any<ValidationContext<PropertySold>>()).Returns(new ValidationResult()
        {
            Errors = new List<ValidationFailure>()
        });
        _addressService.GetByZipCodeAndCreateIfNotExistsAsync(propertySold.ZipCode).Returns(address);
        _propertySoldRepository.CreateAsync(propertySold).ReturnsNull();
        //Act
        var result = await _sut.CreateAsync(propertySold, userId);
        //Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnNull_WhenAddressIsNull()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var address = new Address()
        {
            City = "City",
            Id = Guid.NewGuid(),
            Neighbourhood = "Neighbourhood",
            State = "State",
            StateAcronym = "St",
            Street = "Street",
            ZipCode = "59000000"
        };
        var propertySold = new PropertySold
        {
            Id = Guid.NewGuid(),
            IdPropertyType = Guid.NewGuid(),
            ZipCode = "59000000",
            IdUserSold = userId,
            IdUserCreator = userId,
            UserCreatorDidAgencyOfProperty = true,
            UserCreatorDidSaleOfProperty = true,
            IdAddress = address.Id,
            AddressNumber = "",
            AddressComplement = "",
            NumberOfBedrooms = 0,
            NumberOfSuites = 0,
            NumberOfBathrooms = 0,
            NumberOfParkingSpots = 0,
            PropertyArea = 0,
            YearOfConstruction = 0,
            HasLobby = true,
            DateOfSale = DateTime.Today,
            TimeElapsedToSell = "",
            AnnouncedValue = 10,
            SaleValue = 10,
            CommissionValue = 1
        };
        _propertySoldValidator.ValidateAsync(Arg.Any<ValidationContext<PropertySold>>()).Returns(new ValidationResult()
        {
            Errors = new List<ValidationFailure>()
        });
        _addressService.GetByZipCodeAndCreateIfNotExistsAsync(propertySold.ZipCode).ReturnsNull();
        //Act
        var result = await _sut.CreateAsync(propertySold, userId);
        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenValidationFails()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var address = new Address()
        {
            City = "City",
            Id = Guid.NewGuid(),
            Neighbourhood = "Neighbourhood",
            State = "State",
            StateAcronym = "St",
            Street = "Street",
            ZipCode = "59000000"
        };
        var propertySold = new PropertySold
        {
            Id = Guid.NewGuid(),
            IdPropertyType = Guid.NewGuid(),
            ZipCode = "59000000",
            IdUserSold = userId,
            IdUserCreator = userId,
            UserCreatorDidAgencyOfProperty = true,
            UserCreatorDidSaleOfProperty = true,
            IdAddress = address.Id,
            AddressNumber = "",
            AddressComplement = "",
            NumberOfBedrooms = 0,
            NumberOfSuites = 0,
            NumberOfBathrooms = 0,
            NumberOfParkingSpots = 0,
            PropertyArea = 0,
            YearOfConstruction = 0,
            HasLobby = true,
            DateOfSale = DateTime.Today,
            TimeElapsedToSell = "",
            AnnouncedValue = 10,
            SaleValue = 10,
            CommissionValue = 1
        };
        _propertySoldValidator.ValidateAsync(Arg.Any<ValidationContext<PropertySold>>()).Returns(new ValidationResult()
        {
            Errors = new List<ValidationFailure>()
            {
                new ("Id", "Teste")
            }
        });
        //Act
        var result = async () => await _sut.CreateAsync(propertySold, userId);
        //Assert
        await result.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnPropertySold_WhenPropertySoldIsUpdated()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var address = new Address()
        {
            City = "City",
            Id = Guid.NewGuid(),
            Neighbourhood = "Neighbourhood",
            State = "State",
            StateAcronym = "St",
            Street = "Street",
            ZipCode = "59000000"
        };
        var propertySold = new PropertySold
        {
            Id = Guid.NewGuid(),
            IdPropertyType = Guid.NewGuid(),
            ZipCode = "59000000",
            IdUserSold = userId,
            IdUserCreator = userId,
            UserCreatorDidAgencyOfProperty = true,
            UserCreatorDidSaleOfProperty = true,
            IdAddress = address.Id,
            AddressNumber = "",
            AddressComplement = "",
            NumberOfBedrooms = 0,
            NumberOfSuites = 0,
            NumberOfBathrooms = 0,
            NumberOfParkingSpots = 0,
            PropertyArea = 0,
            YearOfConstruction = 0,
            HasLobby = true,
            DateOfSale = DateTime.Today,
            TimeElapsedToSell = "",
            AnnouncedValue = 10,
            SaleValue = 10,
            CommissionValue = 1
        };
        _propertySoldValidator.ValidateAsync(Arg.Any<ValidationContext<PropertySold>>()).Returns(new ValidationResult()
        {
            Errors = new List<ValidationFailure>()
        });
        _addressService.GetByZipCodeAndCreateIfNotExistsAsync(propertySold.ZipCode).Returns(address);
        _propertySoldRepository.UpdateAsync(propertySold).Returns(propertySold);
        //Act
        var result = await _sut.UpdateAsync(propertySold, userId);
        //Assert
        result.Should().BeEquivalentTo(propertySold);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenPropertySoldIsNotUpdated()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var address = new Address()
        {
            City = "City",
            Id = Guid.NewGuid(),
            Neighbourhood = "Neighbourhood",
            State = "State",
            StateAcronym = "St",
            Street = "Street",
            ZipCode = "59000000"
        };
        var propertySold = new PropertySold
        {
            Id = Guid.NewGuid(),
            IdPropertyType = Guid.NewGuid(),
            ZipCode = "59000000",
            IdUserSold = userId,
            IdUserCreator = userId,
            UserCreatorDidAgencyOfProperty = true,
            UserCreatorDidSaleOfProperty = true,
            IdAddress = address.Id,
            AddressNumber = "",
            AddressComplement = "",
            NumberOfBedrooms = 0,
            NumberOfSuites = 0,
            NumberOfBathrooms = 0,
            NumberOfParkingSpots = 0,
            PropertyArea = 0,
            YearOfConstruction = 0,
            HasLobby = true,
            DateOfSale = DateTime.Today,
            TimeElapsedToSell = "",
            AnnouncedValue = 10,
            SaleValue = 10,
            CommissionValue = 1
        };
        _propertySoldValidator.ValidateAsync(Arg.Any<ValidationContext<PropertySold>>()).Returns(new ValidationResult()
        {
            Errors = new List<ValidationFailure>()
        });
        _addressService.GetByZipCodeAndCreateIfNotExistsAsync(propertySold.ZipCode).Returns(address);
        _propertySoldRepository.UpdateAsync(propertySold).ReturnsNull();
        //Act
        var result = await _sut.UpdateAsync(propertySold, userId);
        //Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenAddressIsNull()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var address = new Address()
        {
            City = "City",
            Id = Guid.NewGuid(),
            Neighbourhood = "Neighbourhood",
            State = "State",
            StateAcronym = "St",
            Street = "Street",
            ZipCode = "59000000"
        };
        var propertySold = new PropertySold
        {
            Id = Guid.NewGuid(),
            IdPropertyType = Guid.NewGuid(),
            ZipCode = "59000000",
            IdUserSold = userId,
            IdUserCreator = userId,
            UserCreatorDidAgencyOfProperty = true,
            UserCreatorDidSaleOfProperty = true,
            IdAddress = address.Id,
            AddressNumber = "",
            AddressComplement = "",
            NumberOfBedrooms = 0,
            NumberOfSuites = 0,
            NumberOfBathrooms = 0,
            NumberOfParkingSpots = 0,
            PropertyArea = 0,
            YearOfConstruction = 0,
            HasLobby = true,
            DateOfSale = DateTime.Today,
            TimeElapsedToSell = "",
            AnnouncedValue = 10,
            SaleValue = 10,
            CommissionValue = 1
        };
        _propertySoldValidator.ValidateAsync(Arg.Any<ValidationContext<PropertySold>>()).Returns(new ValidationResult()
        {
            Errors = new List<ValidationFailure>()
        });
        _addressService.GetByZipCodeAndCreateIfNotExistsAsync(Arg.Any<string>()).ReturnsNull();
        //Act
        var result = await _sut.UpdateAsync(propertySold, userId);
        //Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenValidationFails()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var address = new Address()
        {
            City = "City",
            Id = Guid.NewGuid(),
            Neighbourhood = "Neighbourhood",
            State = "State",
            StateAcronym = "St",
            Street = "Street",
            ZipCode = "59000000"
        };
        var propertySold = new PropertySold
        {
            Id = Guid.NewGuid(),
            IdPropertyType = Guid.NewGuid(),
            ZipCode = "59000000",
            IdUserSold = userId,
            IdUserCreator = userId,
            UserCreatorDidAgencyOfProperty = true,
            UserCreatorDidSaleOfProperty = true,
            IdAddress = address.Id,
            AddressNumber = "",
            AddressComplement = "",
            NumberOfBedrooms = 0,
            NumberOfSuites = 0,
            NumberOfBathrooms = 0,
            NumberOfParkingSpots = 0,
            PropertyArea = 0,
            YearOfConstruction = 0,
            HasLobby = true,
            DateOfSale = DateTime.Today,
            TimeElapsedToSell = "",
            AnnouncedValue = 10,
            SaleValue = 10,
            CommissionValue = 1
        };
        _propertySoldValidator.ValidateAsync(Arg.Any<ValidationContext<PropertySold>>()).Returns(new ValidationResult()
        {
            Errors = new List<ValidationFailure>()
            {
                new ("Id", "Teste")
            }
        });
        //Act
        var result = async () => await _sut.UpdateAsync(propertySold, userId);
        //Assert
        await result.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfPropertySold_WhenPropertySoldExist()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var address = new Address()
        {
            City = "City",
            Id = Guid.NewGuid(),
            Neighbourhood = "Neighbourhood",
            State = "State",
            StateAcronym = "St",
            Street = "Street",
            ZipCode = "59000000"
        };
        var propertySold = new PropertySold
        {
            Id = Guid.NewGuid(),
            IdPropertyType = Guid.NewGuid(),
            ZipCode = "59000000",
            IdUserSold = userId,
            IdUserCreator = userId,
            UserCreatorDidAgencyOfProperty = true,
            UserCreatorDidSaleOfProperty = true,
            IdAddress = address.Id,
            AddressNumber = "",
            AddressComplement = "",
            NumberOfBedrooms = 0,
            NumberOfSuites = 0,
            NumberOfBathrooms = 0,
            NumberOfParkingSpots = 0,
            PropertyArea = 0,
            YearOfConstruction = 0,
            HasLobby = true,
            DateOfSale = DateTime.Today,
            TimeElapsedToSell = "",
            AnnouncedValue = 10,
            SaleValue = 10,
            CommissionValue = 1
        };
        var expectedList = new[] { propertySold };
        _propertySoldRepository.GetAllAsync(Arg.Any<GetAllPropertySoldOptions>()).Returns(expectedList);
        //Act
        var result = await _sut.GetAllAsync(new GetAllPropertySoldOptions());
        //Assert
        result.Should().BeEquivalentTo(expectedList);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoPropertySoldExist()
    {
        //Arrange
        _propertySoldRepository.GetAllAsync(Arg.Any<GetAllPropertySoldOptions>()).Returns(Enumerable.Empty<PropertySold>());
        //Act
        var result = await _sut.GetAllAsync(new GetAllPropertySoldOptions());
        //Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnPropertySold_WhenPropertySoldExist()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var address = new Address()
        {
            City = "City",
            Id = Guid.NewGuid(),
            Neighbourhood = "Neighbourhood",
            State = "State",
            StateAcronym = "St",
            Street = "Street",
            ZipCode = "59000000"
        };
        var propertySold = new PropertySold
        {
            Id = Guid.NewGuid(),
            IdPropertyType = Guid.NewGuid(),
            ZipCode = "59000000",
            IdUserSold = userId,
            IdUserCreator = userId,
            UserCreatorDidAgencyOfProperty = true,
            UserCreatorDidSaleOfProperty = true,
            IdAddress = address.Id,
            AddressNumber = "",
            AddressComplement = "",
            NumberOfBedrooms = 0,
            NumberOfSuites = 0,
            NumberOfBathrooms = 0,
            NumberOfParkingSpots = 0,
            PropertyArea = 0,
            YearOfConstruction = 0,
            HasLobby = true,
            DateOfSale = DateTime.Today,
            TimeElapsedToSell = "",
            AnnouncedValue = 10,
            SaleValue = 10,
            CommissionValue = 1
        };
        _propertySoldRepository.GetByIdAsync(propertySold.Id).Returns(propertySold);
        //Act
        var result = await _sut.GetByIdAsync(propertySold.Id);
        //Assert
        result.Should().BeEquivalentTo(propertySold);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenPropertySoldNotExist()
    {
        //Arrange
        _propertySoldRepository.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();
        //Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());
        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnTrue_WhenPropertySoldIsDeleted()
    {
        //Arrange
        _propertySoldRepository.DeleteByIdAsync(Arg.Any<Guid>(), Arg.Any<Guid>()).Returns(true);
        //Act
        var result = await _sut.DeleteByIdAsync(Guid.NewGuid(), Guid.NewGuid());
        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnFalse_WhenPropertySoldIsNotDeleted()
    {
        //Arrange
        _propertySoldRepository.DeleteByIdAsync(Arg.Any<Guid>(), Arg.Any<Guid>()).Returns(false);
        //Act
        var result = await _sut.DeleteByIdAsync(Guid.NewGuid(), Guid.NewGuid());
        //Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task ExistsByIdAsync_ShouldReturnTrue_WhenPropertySoldExists()
    {
        //Arrange
        _propertySoldRepository.ExistsByIdAsync(Arg.Any<Guid>()).Returns(true);
        //Act
        var result = await _sut.ExistsByIdAsync(Guid.NewGuid());
        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task ExistsByIdAsync_ShouldReturnFalse_WhenPropertySoldNotExists()
    {
        //Arrange
        _propertySoldRepository.ExistsByIdAsync(Arg.Any<Guid>()).Returns(false);
        //Act
        var result = await _sut.ExistsByIdAsync(Guid.NewGuid());
        //Assert
        result.Should().BeFalse();
    }
}