using ChickenBroker.Application.Models;
using ChickenBroker.Application.Repositories.Interfaces;
using ChickenBroker.Application.Services;
using ChickenBroker.Application.Validators;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace ChickenBroker.Application.Tests.Unit;

public class UserServiceTests
{
    private readonly UserService _sut;
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IValidator<User> _userValidator = Substitute.For<IValidator<User>>();
    
    public UserServiceTests()
    {
        _sut = new UserService(_userRepository, _userValidator);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfUsers_WhenUsersExist()
    {
        //Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            Name = "Test",
        };
        var expectedUsers = new[] { user };
        _userRepository.GetAllAsync().Returns(expectedUsers);
        
        //Act
        var result = await _sut.GetAllAsync();
        
        //Assert
        result.Should().BeEquivalentTo(expectedUsers);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenUserDoesNotExist()
    {
        //Arrange
        _userRepository.GetAllAsync().Returns(Enumerable.Empty<User>());
        //Act
        var result = await _sut.GetAllAsync();
        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        //Arrange
        var expectedUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            Name = "Test",
        };
        _userRepository.GetByIdAsync(expectedUser.Id).Returns(expectedUser);
        //Act
        var result = await _sut.GetByIdAsync(expectedUser.Id); 
        //Assert
        result.Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        //Arrange
        _userRepository.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();
        //Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());
        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnTrue_WhenUserIsDeleted()
    {
        //Arrange
        _userRepository.DeleteByIdAsync(Arg.Any<Guid>()).Returns(true);
        //Act
        var result = await _sut.DeleteByIdAsync(Guid.NewGuid());
        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnFalse_WhenUserIsNotDeleted()
    {
        //Arrange
        _userRepository.DeleteByIdAsync(Arg.Any<Guid>()).Returns(false);
        //Act
        var result = await _sut.DeleteByIdAsync(Guid.NewGuid());
        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ExistsByIdAsync_ShouldReturnTrue_WhenUserExists()
    {
        //Arrange
        _userRepository.ExistsByIdAsync(Arg.Any<Guid>()).Returns(true);
        //Act
        var result = await _sut.ExistsByIdAsync(Guid.NewGuid());
        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task ExistsByIdAsync_ShouldReturnFalse_WhenUserNotExists()
    {
        //Arrange
        _userRepository.ExistsByIdAsync(Arg.Any<Guid>()).Returns(false);
        //Act
        var result = await _sut.ExistsByIdAsync(Guid.NewGuid());
        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ExistsByEmailAsync_ShouldReturnTrue_WhenUserExists()
    {
        //Arrange
        _userRepository.ExistsByEmailAsync(Arg.Any<string>()).Returns(true);
        //Act
        var result = await _sut.ExistsByEmailAsync(String.Empty);
        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task ExistsByEmailAsync_ShouldReturnFalse_WhenUserNotExists()
    {
        //Arrange
        _userRepository.ExistsByEmailAsync(Arg.Any<string>()).Returns(false);
        //Act
        var result = await _sut.ExistsByEmailAsync(String.Empty);
        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnTrue_WhenUserIsCreated()
    {
        //Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            Name = "Test",
        };
        _userRepository.CreateAsync(user).Returns(true);
        //Act
        var result = await _sut.CreateAsync(user);
        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_WhenUserIsCreated()
    {
        //Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            Name = "Test",
        };
        _userRepository.CreateAsync(user).Returns(false);
        //Act
        var result = await _sut.CreateAsync(user);
        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUser_WhenUserIsUpdated()
    {
        //Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            Name = "Test",
        };
        _userRepository.ExistsByIdAsync(user.Id).Returns(true);
        _userRepository.UpdateAsync(user).Returns(true);
        //Act
        var result = await _sut.UpdateAsync(user);
        //Assert
        result.Should().BeEquivalentTo(user);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        //Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            Name = "Test",
        };
        _userRepository.ExistsByIdAsync(user.Id).Returns(false);
        _userRepository.UpdateAsync(user).Returns(true);
        //Act
        var result = await _sut.UpdateAsync(user);
        //Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenUserIsNotUpdated()
    {
        //Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            Name = "Test",
        };
        _userRepository.ExistsByIdAsync(user.Id).Returns(true);
        _userRepository.UpdateAsync(user).Returns(false);
        //Act
        var result = await _sut.UpdateAsync(user);
        //Assert
        result.Should().BeNull();
    }
    
}