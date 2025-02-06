using ChickenBroker.Application.Database;
using ChickenBroker.Application.Models;
using ChickenBroker.Application.Repositories.Interfaces;
using Dapper;

namespace ChickenBroker.Application.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public AddressRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Address?> CreateAsync(Address address, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync("""
                                                   INSERT INTO [dbo].[Address]
                                                         ([Id]
                                                         ,[ZipCode]
                                                         ,[Street]
                                                         ,[City]
                                                         ,[State]
                                                         ,[StateAcronym]
                                                         ,[Neighbourhood])
                                                   VALUES
                                                         (@Id
                                                         ,@ZipCode
                                                         ,@Street
                                                         ,@City
                                                         ,@State
                                                         ,@StateAcronym
                                                         ,@Neighbourhood)
                                                   """, address, transaction);
        transaction.Commit();
        
        return result > 0 ? address : null;
    }

    public async Task<Address?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var address = await connection.QueryFirstOrDefaultAsync<Address>("""
            SELECT * FROM [dbo].[Address]
            WHERE [Id] = @Id
            """, new { Id = id});
          
        return address;
    }

    public async Task<Address?> GetByZipCodeAsync(string zipCode, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var address = await connection.QueryFirstOrDefaultAsync<Address>("""
             SELECT * FROM [dbo].[Address]
             WHERE [ZipCode] = @ZipCode
             """, new { ZipCode = zipCode});
          
        return address;
    }

    public async Task<IEnumerable<Address>> GetAllAsync(CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var result = await connection.QueryAsync<Address>(new CommandDefinition("""
            SELECT * FROM [dbo].[Address]
            """, cancellationToken: token));
        
        
        return result;
    }

    public async Task<Address?> UpdateAsync(Address address, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync("""
                                                   UPDATE [dbo].[Address]
                                                   SET [Street] = @Street
                                                      ,[City] = @City
                                                      ,[State] = @State
                                                      ,[StateAcronym] = @StateAcronym
                                                      ,[Neighbourhood] = @Neighbourhood
                                                   WHERE [Id] = @Id
                                                   """, address, transaction);
        transaction.Commit();
        
        return result > 0 ? address : null;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        
        var result = await connection.ExecuteAsync("""
                                                       DELETE FROM [dbo].[Address]
                                                       WHERE [Id] = @Id
                                                       """, new { Id = id }, transaction);
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                                                                               select count(1) from [dbo].[Address] where id = @id
                                                                               """, new { id }, cancellationToken: token));
    }

    public async Task<bool> ExistsByZipCodeAsync(string zipCode, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                                                                               select count(1) from [dbo].[Address] where ZipCode = @ZipCode
                                                                               """, new { ZipCode = zipCode }, cancellationToken: token));
    }
}