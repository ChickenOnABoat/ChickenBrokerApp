using ChickenBroker.Application.Database;
using ChickenBroker.Application.Repositories;
using ChickenBroker.Application.Repositories.Interfaces;
using ChickenBroker.Application.Services;
using ChickenBroker.Application.Services.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ChickenBroker.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPropertyAgencyRepository, PropertyAgencyRepository>();
        services.AddSingleton<IPropertyAgencyService, PropertyAgencyService>();
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<IPropertySoldRepository, PropertySoldRepository>();
        services.AddSingleton<IPropertySoldService, PropertySoldService>();
        services.AddSingleton<IAddressRepository, AddressRepository>();
        services.AddSingleton<IAddressService, AddressService>();
        services.AddSingleton<IPropertyTypeRepository, PropertyTypeRepository>();
        services.AddSingleton<IPropertyTypeService, PropertyTypeService>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new SqlClientConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
    
}