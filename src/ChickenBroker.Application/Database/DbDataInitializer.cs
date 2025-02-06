using Dapper;

namespace ChickenBroker.Application.Database;

public class DbDataInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DbDataInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        
        await connection.ExecuteAsync("""
                                      USE [BrokerApp]
                                      GO
                                      INSERT INTO
                                            [dbo].[PropertyType] ([Id], [Name])
                                      VALUES
                                            (
                                                  '717A3E09-7A83-482A-B841-2FAEEB7740C0',
                                                  'House'
                                            )
                                      INSERT INTO
                                            [dbo].[PropertyType] ([Id], [Name])
                                      VALUES
                                            (
                                                  '86670C3C-9CDD-45ED-913C-42CDA79F0145',
                                                  'Apartment'
                                            )
                                      INSERT INTO
                                            [dbo].[PropertyType] ([Id], [Name])
                                      VALUES
                                            (
                                                  'D9CC1D14-E506-4D3A-B201-91ACC4295D47',
                                                  'Rooftop'
                                            )
                                      INSERT INTO
                                            [dbo].[PropertyType] ([Id], [Name])
                                      VALUES
                                            (
                                                  '652FAF6E-11B4-48D5-8366-809B59A75E89',
                                                  'Comercial Space'
                                            )
                                      INSERT INTO
                                            [dbo].[PropertyType] ([Id], [Name])
                                      VALUES
                                            (
                                                  '2775E67D-E639-4B2B-A93C-17E6ACF60679',
                                                  'Lot'
                                            )
                                      INSERT INTO
                                            [dbo].[PropertyType] ([Id], [Name])
                                      VALUES
                                            (
                                                  'B9F72D09-DABF-4812-892C-B35EAD7FE6F6',
                                                  'Others'
                                            )
                                      GO
                                      """);
    }
}