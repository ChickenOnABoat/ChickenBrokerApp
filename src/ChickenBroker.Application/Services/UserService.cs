using ChickenBroker.Application.Models;
using ChickenBroker.Application.Repositories;
using ChickenBroker.Application.Repositories.Interfaces;
using ChickenBroker.Application.Services.Interfaces;
using FluentValidation;

namespace ChickenBroker.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<User> _userValidator;

    public UserService(IUserRepository userRepository, IValidator<User> userValidator)
    {
        _userRepository = userRepository;
        _userValidator = userValidator;
    }

    public async Task<bool> CreateAsync(User user, CancellationToken token = default)
    {
        await _userValidator.ValidateAndThrowAsync(user, token);
        return await _userRepository.CreateAsync(user, token);
    }

    public Task<User?> GetByIdAsync(Guid id, Guid? userid = default, CancellationToken token = default)
    {
        return _userRepository.GetByIdAsync(id, userid, token);
    }

    public Task<IEnumerable<User>> GetAllAsync(CancellationToken token = default)
    {
        return _userRepository.GetAllAsync(token);
    }

    public async Task<User?> UpdateAsync(User user, CancellationToken token = default)
    {
        var us = await _userRepository.ExistsByIdAsync(user.Id, token);
        if (!us)
        {
            return null;
        }
        var result = await _userRepository.UpdateAsync(user, token);
        return result ? user : null;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _userRepository.DeleteByIdAsync(id, token);
    }

    public Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        return _userRepository.ExistsByIdAsync(id, token);
    }

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken token = default)
    {
        return _userRepository.ExistsByEmailAsync(email, token);
    }
}