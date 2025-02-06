using ChickenBroker.Application.Models;
using ChickenBroker.Application.Repositories.Interfaces;
using ChickenBroker.Application.Services;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SDKBrasilAPI;
using SDKBrasilAPI.Responses;

namespace ChickenBroker.Application.Tests.Unit;

public class AddressServiceTests
{
    private readonly AddressService _sut;
    private readonly IAddressRepository _addressRepository = Substitute.For<IAddressRepository>();
    private readonly IBrasilAPI _brasilApi = Substitute.For<IBrasilAPI>();

    private readonly Address _mockAddress = new Address()
    {
        City = "City",
        Id = Guid.NewGuid(),
        Neighbourhood = "Neighbourhood",
        State = "State",
        StateAcronym = "St",
        Street = "Street",
        ZipCode = "59000000"
    };

    private readonly CEPResponse _mockCepResponse = new CEPResponse()
    {
        CEP = "59000000",
        City = "City",
        Location = new Location()
        {
            Coordinates = new Coordinates()
            {
                Latitude = "59.99",
                Longitude = "90.99"
            },
            Type = "City"
        },
        Neighborhood = "Neighbourhood",
        Street = "Street",
        UF = UF.SP
    };


    private readonly IBGEResponse _mockIbgeResponse = new IBGEResponse()
    {
        IBGEs = new[]
        {
            new IBGE()
            {
                ID = 1,
                Nome = "SP",
                Regiao = new Regiao()
                {
                    ID = 1,
                    Nome = "Regiao",
                    Sigla = "SP"
                },
                Sigla = "SP"
            }
        },
    };

    public AddressServiceTests()
    {
        _sut = new AddressService(_addressRepository, _brasilApi);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnAddress_WhenAddressIsCreated()
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
        
        _brasilApi.CEP_V2(address.ZipCode).Returns(_mockCepResponse);
        _brasilApi.IBGE_UF(_mockCepResponse.UF).Returns(_mockIbgeResponse);
        _addressRepository.CreateAsync(Arg.Do<Address>(x => address = x)).Returns(_mockAddress);
        //Act
        var result = await _sut.CreateAsync(_mockAddress.ZipCode);
        //Assert
        result.Should().BeEquivalentTo(_mockAddress);
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
        
        _brasilApi.CEP_V2(address.ZipCode).Returns(_mockCepResponse);
        _brasilApi.IBGE_UF(_mockCepResponse.UF).Returns(_mockIbgeResponse);
        _addressRepository.CreateAsync(Arg.Do<Address>(x => address = x)).ReturnsNull();
        //Act
        var result = await _sut.CreateAsync(_mockAddress.ZipCode);
        //Assert
        result.Should().BeNull();
    }
    
    
    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenBrasilApiDoesNotFindAddress()
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
        
        _brasilApi.CEP_V2(address.ZipCode).ReturnsNull();
        //Act
        var result = async () => await _sut.CreateAsync(_mockAddress.ZipCode);
        //Assert
        await result.Should().ThrowAsync<Exception>().WithMessage("Address not found");
    }
    
    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenBrasilApiDoesNotFindUF()
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
        
        _brasilApi.CEP_V2(address.ZipCode).Returns(_mockCepResponse);
        _brasilApi.IBGE_UF(_mockCepResponse.UF).ReturnsNull();
        //Act
        var result = async () => await _sut.CreateAsync(_mockAddress.ZipCode);
        //Assert
        await result.Should().ThrowAsync<Exception>().WithMessage("UF not available on Address");
    }

    [Fact]
    public async Task GetByZipCodeAndCreateIfNotExistsAsync_ShouldReturnAddress_WhenAddressExists()
    {
        //Arrange
        _addressRepository.ExistsByZipCodeAsync(_mockAddress.ZipCode).Returns(true);
        _addressRepository.GetByZipCodeAsync(_mockAddress.ZipCode).Returns(_mockAddress);
        //Act
        var result = await _sut.GetByZipCodeAndCreateIfNotExistsAsync(_mockAddress.ZipCode);
        //Assert
        result.Should().BeEquivalentTo(_mockAddress);
    }

    [Fact]
    public async Task GetByZipCodeAndCreateIfNotExistsAsync_ShouldCreateAndReturnAddress_WhenAddressDoesNotExists()
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
        _addressRepository.ExistsByZipCodeAsync(address.ZipCode).Returns(false);
        
        _brasilApi.CEP_V2(address.ZipCode).Returns(_mockCepResponse);
        _brasilApi.IBGE_UF(_mockCepResponse.UF).Returns(_mockIbgeResponse);
        _addressRepository.CreateAsync(Arg.Do<Address>(x => address = x)).Returns(_mockAddress);
        
        //Act
        var result = await _sut.GetByZipCodeAndCreateIfNotExistsAsync(_mockAddress.ZipCode);
        //Assert
        result.Should().BeEquivalentTo(_mockAddress);
    }
    
    [Fact]
    public async Task GetByZipCodeAndCreateIfNotExistsAsync_ShouldReturnNull_WhenAddressDoesNotExistsAndIsNotCreated()
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
        _addressRepository.ExistsByZipCodeAsync(address.ZipCode).Returns(false);
        
        _brasilApi.CEP_V2(address.ZipCode).Returns(_mockCepResponse);
        _brasilApi.IBGE_UF(_mockCepResponse.UF).Returns(_mockIbgeResponse);
        _addressRepository.CreateAsync(Arg.Do<Address>(x => address = x)).ReturnsNull();
        
        //Act
        var result = await _sut.GetByZipCodeAndCreateIfNotExistsAsync(_mockAddress.ZipCode);
        //Assert
        result.Should().BeNull();
    }
    
    
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnAddress_WhenAddressExists()
    {
        //Arrange
        _addressRepository.GetByIdAsync(_mockAddress.Id).Returns(_mockAddress);
        //Act
        var result = await _sut.GetByIdAsync(_mockAddress.Id);
        //Assert
        result.Should().BeEquivalentTo(_mockAddress);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenAddressDoesNotExists()
    {
        //Arrange
        _addressRepository.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();
        //Act
        var result = await _sut.GetByIdAsync(_mockAddress.Id);
        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByZipCodeAsync_ShouldReturnAddress_WhenAddressExists()
    {
        //Arrange
        _addressRepository.GetByZipCodeAsync(_mockAddress.ZipCode).Returns(_mockAddress);
        //Act
        var result = await _sut.GetByZipCodeAsync(_mockAddress.ZipCode);
        //Assert
        result.Should().BeEquivalentTo(_mockAddress);
    }
    
    [Fact]
    public async Task GetByZipCodeAsync_ShouldReturnNull_WhenAddressDoesNotExists()
    {
        //Arrange
        _addressRepository.GetByZipCodeAsync(Arg.Any<string>()).ReturnsNull();
        //Act
        var result = await _sut.GetByZipCodeAsync(_mockAddress.ZipCode);
        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfAddresses_WhenAddressesExist()
    {
        //Arrange
        var expectedAddresses = new []{_mockAddress};
        _addressRepository.GetAllAsync().Returns(expectedAddresses);
        //Act
        var result = await _sut.GetAllAsync();
        //Assert
        result.Should().BeEquivalentTo(expectedAddresses);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenAddressesDoesNotExist()
    {
        //Arrange
        _addressRepository.GetAllAsync().Returns(Enumerable.Empty<Address>());
        //Act
        var result = await _sut.GetAllAsync();
        //Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnAddress_WhenAddressIsUpdated()
    {
        //Arrange
        _addressRepository.UpdateAsync(_mockAddress).Returns(_mockAddress);
        //Act
        var result = await _sut.UpdateAsync(_mockAddress);
        //Assert
        result.Should().BeEquivalentTo(_mockAddress);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenAddressIsNotUpdated()
    {
        //Arrange
        _addressRepository.UpdateAsync(Arg.Any<Address>()).ReturnsNull();
        //Act
        var result = await _sut.UpdateAsync(_mockAddress);
        //Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnTrue_WhenAddressIsDeleted()
    {
        //Arrange
        _addressRepository.DeleteByIdAsync(Arg.Any<Guid>()).Returns(true);
        //Act
        var result = await _sut.DeleteByIdAsync(Guid.NewGuid());
        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnFalse_WhenAddressIsNotDeleted()
    {
        //Arrange
        _addressRepository.DeleteByIdAsync(Arg.Any<Guid>()).Returns(false);
        //Act
        var result = await _sut.DeleteByIdAsync(Guid.NewGuid());
        //Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task ExistsByIdAsync_ShouldReturnTrue_WhenAddressExists()
    {
        //Arrange
        _addressRepository.ExistsByIdAsync(Arg.Any<Guid>()).Returns(true);
        //Act
        var result = await _sut.ExistsByIdAsync(Guid.NewGuid());
        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task ExistsByIdAsync_ShouldReturnFalse_WhenAddressNotExists()
    {
        //Arrange
        _addressRepository.ExistsByIdAsync(Arg.Any<Guid>()).Returns(false);
        //Act
        var result = await _sut.ExistsByIdAsync(Guid.NewGuid());
        //Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task ExistsByZipCodeAsync_ShouldReturnTrue_WhenAddressExists()
    {
        //Arrange
        _addressRepository.ExistsByZipCodeAsync(Arg.Any<string>()).Returns(true);
        //Act
        var result = await _sut.ExistsByZipCodeAsync(String.Empty);
        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task ExistsByZipCodeAsync_ShouldReturnFalse_WhenAddressNotExists()
    {
        //Arrange
        _addressRepository.ExistsByZipCodeAsync(Arg.Any<string>()).Returns(false);
        //Act
        var result = await _sut.ExistsByZipCodeAsync(String.Empty);
        //Assert
        result.Should().BeFalse();
    }
    
}