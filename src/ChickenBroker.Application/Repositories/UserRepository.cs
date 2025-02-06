using ChickenBroker.Application.Database;
using ChickenBroker.Application.Models;
using ChickenBroker.Application.Repositories.Interfaces;
using Dapper;

namespace ChickenBroker.Application.Repositories;

public class UserRepository : IUserRepository
{
    
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public UserRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }


    public async Task<bool> CreateAsync(User user, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync("""
                                                   INSERT INTO [dbo].[UserBroker]
                                                         ([Id]
                                                         ,[Name]
                                                         ,[Email]
                                                         ,[CurrentPropertyAgencyId])
                                                   VALUES
                                                         (@Id
                                                         ,@Name
                                                         ,@Email
                                                         ,@CurrentPropertyAgencyId)
                                                   """, user, transaction);
        transaction.Commit();
        
        return result > 0;
    }

    public async Task<User?> GetByIdAsync(Guid id, Guid? userid = default, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var user = await connection.QueryFirstOrDefaultAsync<User>("""
            SELECT * FROM [dbo].[UserBroker]
            WHERE [Id] = @Id
            """, new { Id = id});
        
        
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var result = await connection.QueryAsync<User>(new CommandDefinition("""
            SELECT * FROM [dbo].[UserBroker]
            """, cancellationToken: token));
        
        
        return result;
    }

    public async Task<bool> UpdateAsync(User user, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync("""
                                                   UPDATE [dbo].[UserBroker]
                                                     SET [Name] = @Name
                                                        ,[Email] = @Email
                                                        ,[CurrentPropertyAgencyId] = @CurrentPropertyAgencyId
                                                   WHERE Id = @Id;
                                                   """, user, transaction);
        transaction.Commit();
        
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         DELETE FROM [dbo].[UserBroker]
                                                                         WHERE [Id] = @Id
                                                                         """, new { Id = id }, transaction,cancellationToken: token));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                                                                               select count(1) from [dbo].[UserBroker] where id = @id
                                                                               """, new { id }, cancellationToken: token));
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                                                                               select count(1) from [dbo].[UserBroker] where Email = @Email
                                                                               """, new { Email = email }, cancellationToken: token));
    }
}