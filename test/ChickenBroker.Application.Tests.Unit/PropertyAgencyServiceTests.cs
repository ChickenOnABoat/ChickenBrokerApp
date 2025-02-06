using ChickenBroker.Application.Models;
using ChickenBroker.Application.Models.Options;
using ChickenBroker.Application.Repositories.Interfaces;
using ChickenBroker.Application.Services;
using ChickenBroker.Application.Services.Interfaces;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ChickenBroker.Application.Tests.Unit;

public class PropertyAgencyServiceTests
{
    private readonly PropertyAgencyService _sut;
    private readonly IPropertyAgencyRepository _propertyAgencyRepository = Substitute.For<IPropertyAgencyRepository>();
    private readonly IAddressService _addressService = Substitute.For<IAddressService>();
    private readonly IValidator<PropertyAgency> _propertyAgencyValidator = Substitute.For<IValidator<PropertyAgency>>();
    private readonly IValidator<GetAllPropertyAgencyOptions> _getAllPropertyAgencyOptionsValidator = Substitute.For<IValidator<GetAllPropertyAgencyOptions>>();

    

    public PropertyAgencyServiceTests()
    {
        _sut = new PropertyAgencyService(_propertyAgencyRepository, _propertyAgencyValidator, _addressService, _getAllPropertyAgencyOptionsValidator);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnPropertyAgency_WhenPropertyAgencyIsCreated()
    {
        //Arrange
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
        var propertyAgency = new PropertyAgency()
        {
            Id = Guid.NewGuid(),
            Address = address,
            AddressComplement = null,
            AddressNumber = null,
            ContactNumber = null,
            IdAddress = address.Id,
            IdentificationDocumentNumber = "teste",
            Name = "Teste",
            ZipCode = address.ZipCode
        };
        
        _propertyAgencyValidator.When(x => x.ValidateAndThrowAsync(propertyAgency)).Do(_ => { });
        _addressService.GetByZipCodeAndCreateIfNotExistsAsync(propertyAgency.ZipCode).Returns(address);
        _propertyAgencyRepository.CreateAsync(propertyAgency).Returns(propertyAgency);
        //Act
        var result = await _sut.CreateAsync(propertyAgency);
        //Assert
        result.Should().BeEquivalentTo(propertyAgency);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnNull_WhenAddressIsNull()
    {
        //Arrange
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
        var propertyAgency = new PropertyAgency()
        {
            Id = Guid.NewGuid(),
            Address = address,
            AddressComplement = null,
            AddressNumber = null,
            ContactNumber = null,
            IdAddress = address.Id,
            IdentificationDocumentNumber = "teste",
            Name = "Teste",
            ZipCode = address.ZipCode
        };
        _propertyAgencyValidator.When(x => x.ValidateAndThrowAsync(Arg.Any<PropertyAgency>())).Do(_ => { });
        _addressService.GetByZipCodeAndCreateIfNotExistsAsync(Arg.Any<string>()).ReturnsNull();
        _propertyAgencyRepository.CreateAsync(Arg.Any<PropertyAgency>()).ReturnsNull();
        //Act
        var result = await _sut.CreateAsync(propertyAgency);
        //Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnNull_WhenAddressIsNotCreated()
    {
        //Arrange
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
        var propertyAgency = new PropertyAgency()
        {
            Id = Guid.NewGuid(),
            Address = address,
            AddressComplement = null,
            AddressNumber = null,
            ContactNumber = null,
            IdAddress = address.Id,
            IdentificationDocumentNumber = "teste",
            Name = "Teste",
            ZipCode = address.ZipCode
        };

        _propertyAgencyValidator.When(x => x.ValidateAndThrowAsync(propertyAgency)).Do(_ => { });
        _addressService.GetByZipCodeAndCreateIfNotExistsAsync(propertyAgency.ZipCode).Returns(address);
        _propertyAgencyRepository.CreateAsync(propertyAgency).ReturnsNull();
        //Act
        var result = await _sut.CreateAsync(propertyAgency);
        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPropertyAgency_WhenPropertyAgencyExists()
    {
        //Arrange
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
        var propertyAgency = new PropertyAgency()
        {
            Id = Guid.NewGuid(),
            Address = address,
            AddressComplement = null,
            AddressNumber = null,
            ContactNumber = null,
            IdAddress = address.Id,
            IdentificationDocumentNumber = "teste",
            Name = "Teste",
            ZipCode = address.ZipCode
        };
        
        _propertyAgencyRepository.GetByIdAsync(propertyAgency.Id).Returns(propertyAgency);
        
        //Act
        var result = await _sut.GetByIdAsync(propertyAgency.Id);
        //Assert
        result.Should().BeEquivalentTo(propertyAgency);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenPropertyAgencyNotExists()
    {
        //Arrange
        _propertyAgencyRepository.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();
        
        //Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());
        
        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfPropertyAgencies_WhenPropertyAgencyExists()
    {
        //Arrange
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
        var propertyAgency = new PropertyAgency()
        {
            Id = Guid.NewGuid(),
            Address = address,
            AddressComplement = null,
            AddressNumber = null,
            ContactNumber = null,
            IdAddress = address.Id,
            IdentificationDocumentNumber = "teste",
            Name = "Teste",
            ZipCode = address.ZipCode
        };
        var expectedList = new[] { propertyAgency };
        
        _getAllPropertyAgencyOptionsValidator.WhenForAnyArgs(x => x.ValidateAndThrowAsync(Arg.Any<GetAllPropertyAgencyOptions>())).Do(_ => { });
        _propertyAgencyRepository.GetAllAsync(Arg.Any<GetAllPropertyAgencyOptions>()).Returns(expectedList);
        //Act
        var result = await _sut.GetAllAsync(new GetAllPropertyAgencyOptions());
        //Assert
        result.Should().BeEquivalentTo(expectedList);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenPropertyAgencyNotExists()
    {
        //Arrange
        _getAllPropertyAgencyOptionsValidator.WhenForAnyArgs(x => x.ValidateAndThrowAsync(Arg.Any<GetAllPropertyAgencyOptions>())).Do(_ => { });
        _propertyAgencyRepository.GetAllAsync(Arg.Any<GetAllPropertyAgencyOptions>()).Returns(Enumerable.Empty<PropertyAgency>());
        //Act
        var result = await _sut.GetAllAsync(new GetAllPropertyAgencyOptions());
        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCountAllAsync_ShouldReturnCountOfPropertyAgencies_WhenInvoked()
    {
        //Arrange
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
        var propertyAgency = new PropertyAgency()
        {
            Id = Guid.NewGuid(),
            Address = address,
            AddressComplement = null,
            AddressNumber = null,
            ContactNumber = null,
            IdAddress = address.Id,
            IdentificationDocumentNumber = "teste",
            Name = "Teste",
            ZipCode = address.ZipCode
        };
        var expectedList = new[] { propertyAgency };
        _propertyAgencyRepository
            .GetCountAllAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(expectedList.Length);
        //Act
        var result = await _sut.GetCountAllAsync(String.Empty, String.Empty, String.Empty, String.Empty);
        //Assert
        result.Should().Be(1);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnPropertyAgency_WhenPropertyAgencyIsUpdated()
    {
        //Arrange
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
        var propertyAgency = new PropertyAgency()
        {
            Id = Guid.NewGuid(),
            Address = address,
            AddressComplement = null,
            AddressNumber = null,
            ContactNumber = null,
            IdAddress = address.Id,
            IdentificationDocumentNumber = "teste",
            Name = "Teste",
            ZipCode = address.ZipCode
        };
        _propertyAgencyValidator.When(x => x.ValidateAndThrowAsync(propertyAgency)).Do(_ => { });
        _addressService.GetByZipCodeAndCreateIfNotExistsAsync(propertyAgency.ZipCode).Returns(address);
        _propertyAgencyRepository.UpdateAsync(propertyAgency).Returns(propertyAgency);
        //Act
        var result = await _sut.UpdateAsync(propertyAgency);
        //Assert
        result.Should().BeEquivalentTo(propertyAgency);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenPropertyAgencyIsNotUpdated()
    {
        //Arrange
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
        var propertyAgency = new PropertyAgency()
        {
            Id = Guid.NewGuid(),
            Address = address,
            AddressComplement = null,
            AddressNumber = null,
            ContactNumber = null,
            IdAddress = address.Id,
            IdentificationDocumentNumber = "teste",
            Name = "Teste",
            ZipCode = address.ZipCode
        };
        _propertyAgencyValidator.When(x => x.ValidateAndThrowAsync(propertyAgency)).Do(_ => { });
        _addressService.GetByZipCodeAndCreateIfNotExistsAsync(propertyAgency.ZipCode).Returns(address);
        _propertyAgencyRepository.UpdateAsync(propertyAgency).ReturnsNull();
        //Act
        var result = await _sut.UpdateAsync(propertyAgency);
        //Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenAddressIsNull()
    {
        //Arrange
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
        var propertyAgency = new PropertyAgency()
        {
            Id = Guid.NewGuid(),
            Address = address,
            AddressComplement = null,
            AddressNumber = null,
            ContactNumber = null,
            IdAddress = address.Id,
            IdentificationDocumentNumber = "teste",
            Name = "Teste",
            ZipCode = address.ZipCode
        };
        _propertyAgencyValidator.When(x => x.ValidateAndThrowAsync(propertyAgency)).Do(_ => { });
        _addressService.GetByZipCodeAndCreateIfNotExistsAsync(propertyAgency.ZipCode).ReturnsNull();
        //Act
        var result = await _sut.UpdateAsync(propertyAgency);
        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnTrue_WhenPropertyAgencyIsDeleted()
    {
        //Arrange
        _propertyAgencyRepository.DeleteByIdAsync(Arg.Any<Guid>()).Returns(true);
        //Act
        var result = await _sut.DeleteByIdAsync(Guid.NewGuid());
        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnFalse_WhenPropertyAgencyIsNotDeleted()
    {
        //Arrange
        _propertyAgencyRepository.DeleteByIdAsync(Arg.Any<Guid>()).Returns(false);
        //Act
        var result = await _sut.DeleteByIdAsync(Guid.NewGuid());
        //Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task ExistsByIdAsync_ShouldReturnTrue_WhenPropertyAgencyExists()
    {
        //Arrange
        _propertyAgencyRepository.ExistsByIdAsync(Arg.Any<Guid>()).Returns(true);
        //Act
        var result = await _sut.ExistsByIdAsync(Guid.NewGuid());
        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task ExistsByIdAsync_ShouldReturnFalse_WhenPropertyAgencyNotExists()
    {
        //Arrange
        _propertyAgencyRepository.ExistsByIdAsync(Arg.Any<Guid>()).Returns(false);
        //Act
        var result = await _sut.ExistsByIdAsync(Guid.NewGuid());
        //Assert
        result.Should().BeFalse();
    }
}