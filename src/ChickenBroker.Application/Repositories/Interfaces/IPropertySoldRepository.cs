using ChickenBroker.Application.Models;
using ChickenBroker.Application.Models.Options;

namespace ChickenBroker.Application.Repositories.Interfaces;

public interface IPropertySoldRepository
{
    Task<PropertySold?> CreateAsync(PropertySold propertySold, CancellationToken token = default);
    
    Task<PropertySold?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default);
    
    //Task<Movie?> GetBySlugAsync(string slug, Guid? userid = default, CancellationToken token = default);
    
    Task<IEnumerable<PropertySold>> GetAllAsync(GetAllPropertySoldOptions options, CancellationToken token = default);
    
    Task<PropertySold?> UpdateAsync(PropertySold propertySold, CancellationToken token = default);
    
    Task<bool> DeleteByIdAsync(Guid id, Guid userId, CancellationToken token = default);
    
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
    
    Task<bool> PropertySoldMatchCreatorAndCurrentIdAsync(Guid propertySoldId, Guid currentUserId, CancellationToken token = default);

    //Task<int> GetCountAsync(string? title, int? yearOfRelease, CancellationToken token = default);
}