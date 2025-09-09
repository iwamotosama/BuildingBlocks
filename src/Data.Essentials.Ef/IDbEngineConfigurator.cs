namespace Nikuman.BuildingBlocks.Data.Essentials.Ef;

/// <summary>
/// Represents database engine-specific configuration actions
/// </summary>
/// <typeparam name="TContext">The <see cref="Type"/> of the context being configured</typeparam>
public interface IDbEngineConfigurator<TContext>
    where TContext : DbContext
{
    /// <summary>
    /// Performs model convention configurations
    /// </summary>
    /// <param name="configurationBuilder">The builder used to perform the configuration</param>
    void ConfigureConventions(ModelConfigurationBuilder configurationBuilder);

    /// <summary>
    /// Performs context configurations
    /// </summary>
    /// <param name="optionsBuilder">The builder used to perform the configuration</param>
    void Configure(DbContextOptionsBuilder<TContext> optionsBuilder);
}
