using ChickenBroker.Application.Models;

namespace ChickenBroker.Application.Services.Interfaces;

public interface IUserService
{
    Task<bool> CreateAsync(User user, CancellationToken token = default);
    
    Task<User?> GetByIdAsync(Guid id, Guid? userid = default, CancellationToken token = default);
    
    //Task<Movie?> GetBySlugAsync(string slug, Guid? userid = default, CancellationToken token = default);
    
    Task<IEnumerable<User>> GetAllAsync(CancellationToken token = default);
    
    Task<User?> UpdateAsync(User user, CancellationToken token = default);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
    
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken token = default);
}