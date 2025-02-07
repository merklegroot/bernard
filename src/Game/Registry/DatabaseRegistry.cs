using DuckDB.NET.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Registry;

public interface IDbConnectionFactory
{
    DuckDBConnection Create();
}

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly DuckDBConnection _connection;
    public DbConnectionFactory(DuckDBConnection connection) =>
        _connection = connection;
    
    public DuckDBConnection Create() =>
        _connection;
}

public static class DatabaseRegistry
{
    public static IServiceCollection RegisterDatabase(this IServiceCollection services)
    {
        const string connectionString = "Data Source=:memory:";
        
        var connection = new DuckDBConnection(connectionString);
        services.AddSingleton(connection);
        
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        
        return services;
    }
}