using System.Data;
using Microsoft.Data.SqlClient;

namespace ChickenBroker.Application.Database;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}

public class SqlClientConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlClientConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }


    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }
}