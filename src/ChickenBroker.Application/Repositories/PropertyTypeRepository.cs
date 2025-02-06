using ChickenBroker.Application.Database;
using ChickenBroker.Application.Models;
using ChickenBroker.Application.Repositories.Interfaces;
using Dapper;

namespace ChickenBroker.Application.Repositories;

public class PropertyTypeRepository : IPropertyTypeRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public PropertyTypeRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<PropertyType>> GetAllAsync(CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        
        var result = await connection.QueryAsync<PropertyType>("SELECT * FROM PropertyType");
        
        return result;
    }
}