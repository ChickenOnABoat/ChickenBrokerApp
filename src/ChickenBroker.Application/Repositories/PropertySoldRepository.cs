using ChickenBroker.Application.Database;
using ChickenBroker.Application.Models;
using ChickenBroker.Application.Models.Options;
using ChickenBroker.Application.Repositories.Interfaces;
using Dapper;

namespace ChickenBroker.Application.Repositories;

public class PropertySoldRepository : IPropertySoldRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public PropertySoldRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }


    public async Task<PropertySold?> CreateAsync(PropertySold propertySold, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync("""
                                                   INSERT INTO [dbo].[PropertySold]
                                                         ([Id]
                                                         ,[IdUserSold]
                                                         ,[IdUserCreator]
                                                         ,[UserCreatorDidAgencyOfProperty]
                                                         ,[UserCreatorDidSaleOfProperty]
                                                         ,[IdPropertyType]
                                                         ,[IdAddress]
                                                         ,[AddressNumber]
                                                         ,[AddressComplement]
                                                         ,[NumberOfBedrooms]
                                                         ,[NumberOfSuites]
                                                         ,[NumberOfBathrooms]
                                                         ,[NumberOfParkingSpots]
                                                         ,[PropertyArea]
                                                         ,[YearOfConstruction]
                                                         ,[HasLobby]
                                                         ,[DateOfSale]
                                                         ,[TimeElapsedToSell]
                                                         ,[AnnouncedValue]
                                                         ,[SaleValue]
                                                         ,[CommissionValue])
                                                   VALUES
                                                         (@Id
                                                         ,@IdUserSold
                                                         ,@IdUserCreator
                                                         ,@UserCreatorDidAgencyOfProperty
                                                         ,@UserCreatorDidSaleOfProperty
                                                         ,@IdPropertyType
                                                         ,@IdAddress
                                                         ,@AddressNumber
                                                         ,@AddressComplement
                                                         ,@NumberOfBedrooms
                                                         ,@NumberOfSuites
                                                         ,@NumberOfBathrooms
                                                         ,@NumberOfParkingSpots
                                                         ,@PropertyArea
                                                         ,@YearOfConstruction
                                                         ,@HasLobby
                                                         ,@DateOfSale
                                                         ,@TimeElapsedToSell
                                                         ,@AnnouncedValue
                                                         ,@SaleValue
                                                         ,@CommissionValue)
                                                   """, propertySold, transaction);
        transaction.Commit();
        
        return result > 0 ? propertySold : null;
    }

    public async Task<PropertySold?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var propertySold = await connection.QueryFirstOrDefaultAsync<PropertySold>("""
            SELECT * FROM [dbo].[PropertySold] 
            WHERE [Id] = @Id AND [IdUserCreator] = @userId
            """, new { Id = id, userId});
        return propertySold;
    }
    
    public async Task<IEnumerable<PropertySold>> GetAllAsync(GetAllPropertySoldOptions options,CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        
        string sortQuery = String.Empty;

        if (options.SortField is not null)
        {
            string tableSort = "[PropertySold]";
            if (options.SortField.Equals("ZipCode") || options.SortField.Equals("City"))
            {
                tableSort = "[Address]";
            }
            sortQuery = $"""
                         order by {tableSort}.{options.SortField} {(options.SortOrder == FieldSortOrder.Ascending ? "asc" : "desc" )}
                         """;
        }
        
        var result = await connection.QueryAsync<PropertySold, Address, PropertySold>($"""
            SELECT [PropertySold].*
            ,[Address].[Id]
            ,[Address].[ZipCode]
            ,[Address].[Street]
            ,[Address].[City]
            ,[Address].[State]
            ,[Address].[StateAcronym]
            ,[Address].[Neighbourhood]
            FROM [dbo].[PropertySold] 
            INNER JOIN [Address] On [Address].[Id] = [PropertySold].[IdAddress]
            where [PropertySold].[IdUserCreator] = @UserId
            AND (@ZipCode is null or [Address].[ZipCode] = @ZipCode) 
            AND (@City is null or [Address].[City] like ('%' + @City + '%'))
            AND (@AnnouncedValue is null or [PropertySold].[AnnouncedValue] = @AnnouncedValue)
            AND (@AnnouncedValueGreaterOrEqual is null or [PropertySold].[AnnouncedValue] >= @AnnouncedValueGreaterOrEqual)
            AND (@AnnouncedValueLesserOrEqual is null or [PropertySold].[AnnouncedValue] <= @AnnouncedValueLesserOrEqual) {sortQuery}
            """, (property, address) =>
        {
            property.Address = address; 
            return property;
        }, param: options, splitOn: "Id");
        
        
        return result;
    }

    public async Task<PropertySold?> UpdateAsync(PropertySold propertySold, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync("""
                                                   UPDATE [dbo].[PropertySold]
                                                     SET [IdUserSold] = @IdUserSold
                                                        ,[UserCreatorDidAgencyOfProperty] = @UserCreatorDidAgencyOfProperty
                                                        ,[UserCreatorDidSaleOfProperty] = @UserCreatorDidSaleOfProperty
                                                        ,[IdPropertyType] = @IdPropertyType
                                                        ,[IdAddress] = @IdAddress
                                                        ,[AddressNumber] = @AddressNumber
                                                        ,[AddressComplement] = @AddressComplement
                                                        ,[NumberOfBedrooms] = @NumberOfBedrooms
                                                        ,[NumberOfSuites] = @NumberOfSuites
                                                        ,[NumberOfBathrooms] = @NumberOfBathrooms
                                                        ,[NumberOfParkingSpots] = @NumberOfParkingSpots
                                                        ,[PropertyArea] = @PropertyArea
                                                        ,[YearOfConstruction] = @YearOfConstruction
                                                        ,[HasLobby] = @HasLobby
                                                        ,[DateOfSale] = @DateOfSale
                                                        ,[TimeElapsedToSell] = @TimeElapsedToSell
                                                        ,[AnnouncedValue] = @AnnouncedValue
                                                        ,[SaleValue] = @SaleValue
                                                        ,[CommissionValue] = @CommissionValue
                                                   WHERE Id = @Id;
                                                   """, propertySold, transaction);
        transaction.Commit();
        
        return result > 0 ? propertySold : null;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, Guid userId,CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         DELETE FROM [dbo].[PropertySold]
                                                                         WHERE [Id] = @Id and [IdUserCreator] = @userId
                                                                         """, new { Id = id, userId }, transaction,cancellationToken: token));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                                                                               select count(1) from [dbo].[PropertySold] where id = @id
                                                                               """, new { id }, cancellationToken: token));
    }

    public async Task<bool> PropertySoldMatchCreatorAndCurrentIdAsync(Guid propertySoldId, Guid currentUserId,
        CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                                                                               select count(1) from [dbo].[PropertySold] where id = @id and [IdUserCreator] = @currentUserId
                                                                               """, new { id = propertySoldId, currentUserId  }, cancellationToken: token));
    }
}