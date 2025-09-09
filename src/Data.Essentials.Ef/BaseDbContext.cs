namespace Nikuman.BuildingBlocks.Data.Essentials.Ef;

/// <summary>
/// A base <see cref="DbContext"/> that supports strategized configuration
/// </summary>
/// <param name="options">The <see cref="DbContextOptions"/> to be used</param>
/// <param name="conventionsStrategy">Stragety to configure model conventions</param>
public abstract partial class BaseDbContext(
    DbContextOptions options,
    BaseDbContext.ConventionsConfigurationStrategy conventionsStrategy) :
    DbContext(options)
{
    /// <summary>
    /// A delegate for configuring model conventions
    /// </summary>
    /// <param name="configurationBuilder">The builder used to perform the configuration</param>
    public delegate void ConventionsConfigurationStrategy(ModelConfigurationBuilder configurationBuilder);

    private readonly ConventionsConfigurationStrategy _conventionsStrategy = conventionsStrategy;

    /// <inheritdoc/>
    protected sealed override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        => _conventionsStrategy(configurationBuilder);
}
