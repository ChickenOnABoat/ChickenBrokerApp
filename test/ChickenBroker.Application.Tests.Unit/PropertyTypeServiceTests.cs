using ChickenBroker.Application.Models;
using ChickenBroker.Application.Repositories.Interfaces;
using ChickenBroker.Application.Services;
using FluentAssertions;
using NSubstitute;

namespace ChickenBroker.Application.Tests.Unit;

public class PropertyTypeServiceTests
{
    private readonly PropertyTypeService _sut;
    private readonly IPropertyTypeRepository _propertyTypeRepository = Substitute.For<IPropertyTypeRepository>();

    public PropertyTypeServiceTests()
    {
        _sut = new PropertyTypeService(_propertyTypeRepository);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfPropertyTypes_WhenPropertyTypesExist()
    {
        //Arrange
        var propertyType = new PropertyType
        {
            Id = Guid.NewGuid(),
            Name = "Test"
        };
        var expectedPropertyTypes = new [] { propertyType };
        _propertyTypeRepository.GetAllAsync().Returns(expectedPropertyTypes);
        //Act
        var result = await _sut.GetAllAsync();
        //Assert
        result.Should().BeEquivalentTo(expectedPropertyTypes);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenPropertyTypesNotExist()
    {
        //Arrange
        _propertyTypeRepository.GetAllAsync().Returns(Enumerable.Empty<PropertyType>());
        //Act
        var result = await _sut.GetAllAsync();
        //Assert
        result.Should().BeEmpty();
    }
}