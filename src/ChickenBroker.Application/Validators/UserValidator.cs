using ChickenBroker.Application.Models;
using ChickenBroker.Application.Repositories;
using ChickenBroker.Application.Repositories.Interfaces;
using FluentValidation;

namespace ChickenBroker.Application.Validators;

public class UserValidator : AbstractValidator<User>
{
    private readonly IUserRepository _userRepository;
    
    public UserValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        RuleFor(user => user.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(s => s.Email).MustAsync(ValidateUser).WithMessage("Email is already being used");
    }
    
    private async Task<bool> ValidateUser(User user, string email,CancellationToken token = default)
    {
        var existingUser = await _userRepository.ExistsByEmailAsync(email);
        return !existingUser;
    }
}