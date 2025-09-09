using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Nikuman.BuildingBlocks.Data.Essentials.Ef.Postgres;

/// <summary>
/// Encapsulates Entity Framework configurations for a PostgreSQL database
/// </summary>
/// <typeparam name="TContext">The <see cref="Type"/> of the <see cref="DbContext"/> being configured</typeparam>
/// <param name="connection">The connection string to the PostgreSQL database</param>
/// <param name="migrationsAssembly">The <see cref="Assembly"/> containing EF migrations</param>
public class PostgresConfiguration<TContext>(string connection, Assembly migrationsAssembly)
    : IDbEngineConfigurator<TContext>
        where TContext : DbContext
{
    private readonly string _connection = connection;
    private readonly Assembly _migrationsAssembly = migrationsAssembly;
    
    /// <inheritdoc/>
    public void Configure(DbContextOptionsBuilder<TContext> optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connection, x =>
        {
            x.MigrationsAssembly(_migrationsAssembly.FullName);
        });
    }

    /// <inheritdoc/>
    public void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // nothing currently
    }
}
