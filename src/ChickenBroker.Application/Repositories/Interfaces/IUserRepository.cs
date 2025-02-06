using ChickenBroker.Application.Models;

namespace ChickenBroker.Application.Repositories.Interfaces;

public interface IUserRepository
{
    Task<bool> CreateAsync(User user, CancellationToken token = default);

    Task<User?> GetByIdAsync(Guid id, Guid? userid = default, CancellationToken token = default);
    
    Task<IEnumerable<User>> GetAllAsync(CancellationToken token = default);
    
    Task<bool> UpdateAsync(User user, CancellationToken token = default);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken token = default);
}