using ChickenBroker.Application.Database;
using ChickenBroker.Application.Models;
using ChickenBroker.Application.Models.Options;
using ChickenBroker.Application.Repositories.Interfaces;
using Dapper;

namespace ChickenBroker.Application.Repositories;


public class PropertyAgencyRepository : IPropertyAgencyRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public PropertyAgencyRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<PropertyAgency?> CreateAsync(PropertyAgency propertyAgency, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync("""
                                             INSERT INTO [dbo].[PropertyAgency]
                                                   ([Id]
                                                   ,[Name]
                                                   ,[IdentificationDocumentNumber]
                                                   ,[AddressNumber]
                                                   ,[AddressComplement]
                                                   ,[IdAddress]
                                                   ,[ContactNumber])
                                             VALUES
                                                   (@Id
                                                   ,@Name
                                                   ,@IdentificationDocumentNumber
                                                   ,@AddressNumber
                                                   ,@AddressComplement
                                                   ,@IdAddress
                                                   ,@ContactNumber)
                                             """, propertyAgency, transaction);
        transaction.Commit();
        
        return result > 0 ? propertyAgency : null;
    }
    

    public async Task<PropertyAgency?> GetByIdAsync(Guid id, Guid? userid = default, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var propertyAgency = await connection.QueryFirstOrDefaultAsync<PropertyAgency>("""
                                                                   SELECT [Id]
                                                                       ,[Name]
                                                                       ,[IdentificationDocumentNumber]
                                                                       ,[AddressNumber]
                                                                       ,[AddressComplement]
                                                                       ,[IdAddress]
                                                                       ,[ContactNumber]
                                                                   FROM [dbo].[PropertyAgency]
                                                                   WHERE [Id] = @Id
                                                                   """, new { Id = id});
        if (propertyAgency is null)
        {
            return null;
        }
        var addressPropertyAgency = await connection.QueryFirstOrDefaultAsync<Address>("""
            SELECT * FROM [dbo].[Address]
            Where Id = @Id
            """, new { Id = propertyAgency.IdAddress });
        propertyAgency.Address = addressPropertyAgency;    
        return propertyAgency;
    }

    public async Task<IEnumerable<PropertyAgency>> GetAllAsync(GetAllPropertyAgencyOptions options,CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        
        string sortQuery = "order by [PropertyAgency].[Id]";
        if (options.SortField is not null)
        {
            string tableSort = "[PropertyAgency]";
            if (options.SortField.Equals("ZipCode") || options.SortField.Equals("City"))
            {
                tableSort = "[Address]";
            }
            sortQuery = $"""
                         order by {tableSort}.{options.SortField} {(options.SortOrder == FieldSortOrder.Ascending ? "asc" : "desc" )}
                         """;
        }
        
        var result = await connection.QueryAsync<PropertyAgency, Address, PropertyAgency>($"""
            SELECT [PropertyAgency].[Id]
            ,[PropertyAgency].[Name]
            ,[PropertyAgency].[IdentificationDocumentNumber]
            ,[PropertyAgency].[AddressNumber]
            ,[PropertyAgency].[AddressComplement]
            ,[PropertyAgency].[IdAddress]
            ,[PropertyAgency].[ContactNumber]
            ,[Address].[Id]
            ,[Address].[ZipCode]
            ,[Address].[Street]
            ,[Address].[City]
            ,[Address].[State]
            ,[Address].[StateAcronym]
            ,[Address].[Neighbourhood] FROM [dbo].[PropertyAgency]
            INNER JOIN [Address] On [Address].[Id] = [PropertyAgency].[IdAddress]
            WHERE (@Name is null or [PropertyAgency].[Name] like ('%' + @Name + '%'))
            AND (@City is null or [Address].[City] like ('%' + @City + '%'))
            AND (@IdentificationDocumentNumber is null or [PropertyAgency].[IdentificationDocumentNumber] like ('%' + @IdentificationDocumentNumber + '%'))
            AND (@ZipCode is null or [Address].[ZipCode] = @ZipCode) {sortQuery} 
            OFFSET @pageOffset ROWS
            FETCH NEXT @pageSize ROWS ONLY;
            """, (property, address) =>
        {
            property.Address = address; 
            return property;
        }, param: new {
                options.Name,
                options.City,
                options.IdentificationDocumentNumber,
                options.ZipCode,
                pageSize = options.PageSize,
                pageOffset = (options.Page - 1) * options.PageSize,
        }, splitOn: "Id");
        return result;
    }

    public async Task<int> GetCountAllAsync(string? name, string? city, string? identificationDocumentNumber, string? zipCode,
        CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        
        return await connection.QuerySingleAsync<int>(new CommandDefinition("""
                                                                            select count([PropertyAgency].[Id]) from PropertyAgency
                                                                            INNER JOIN [Address] On [Address].[Id] = [PropertyAgency].[IdAddress]
                                                                            WHERE (@name is null or [PropertyAgency].[Name] like ('%' + @name + '%'))
                                                                            AND (@city is null or [Address].[City] like ('%' + @city + '%'))
                                                                            AND (@identificationDocumentNumber is null or [PropertyAgency].[IdentificationDocumentNumber] like ('%' + @identificationDocumentNumber + '%'))
                                                                            AND (@zipCode is null or [Address].[ZipCode] = @zipCode)
                                                                            """, new
        {
            name,
            city,
            identificationDocumentNumber,
            zipCode
        }, cancellationToken: token));
    }

    public async Task<PropertyAgency?> UpdateAsync(PropertyAgency propertyAgency, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync("""
                                                   UPDATE [dbo].[PropertyAgency]
                                                     SET [Name] = @Name
                                                           ,[IdentificationDocumentNumber] = @IdentificationDocumentNumber
                                                           ,[AddressNumber] = @AddressNumber
                                                           ,[AddressComplement] = @AddressComplement
                                                           ,[IdAddress] = @IdAddress
                                                           ,[ContactNumber] = @ContactNumber
                                                   WHERE Id = @Id
                                                   """, propertyAgency, transaction);
        transaction.Commit();
        
        return result > 0 ? propertyAgency: null;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         DELETE FROM [dbo].[PropertyAgency]
                                                                         WHERE [Id] = @Id
                                                                         """, new { Id = id }, transaction,cancellationToken: token));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                                                                               select count(1) from [dbo].[PropertyAgency] where id = @id
                                                                               """, new { id }, cancellationToken: token));
    }
}