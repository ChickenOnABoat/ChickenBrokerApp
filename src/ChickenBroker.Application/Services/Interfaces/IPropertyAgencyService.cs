using ChickenBroker.Application.Models;
using ChickenBroker.Application.Models.Options;

namespace ChickenBroker.Application.Services.Interfaces;

public interface IPropertyAgencyService
{
    Task<PropertyAgency?> CreateAsync(PropertyAgency propertyAgency, CancellationToken token = default);
    
    Task<PropertyAgency?> GetByIdAsync(Guid id, Guid? userid = default, CancellationToken token = default);
    
    //Task<Movie?> GetBySlugAsync(string slug, Guid? userid = default, CancellationToken token = default);
    
    Task<IEnumerable<PropertyAgency>> GetAllAsync(GetAllPropertyAgencyOptions options,CancellationToken token = default);
    Task<int> GetCountAllAsync(string? name, string? city, string? identificationDocumentNumber, string? zipCode,CancellationToken token = default);
    
    Task<PropertyAgency?> UpdateAsync(PropertyAgency propertyAgency, CancellationToken token = default);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
    
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);

    //Task<int> GetCountAsync(string? title, int? yearOfRelease, CancellationToken token = default);
}