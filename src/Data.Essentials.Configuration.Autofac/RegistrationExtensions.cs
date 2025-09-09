using Autofac;
using Autofac.Builder;
using Microsoft.EntityFrameworkCore;
using Nikuman.BuildingBlocks.Data.Essentials.Ef;

namespace Nikuman.BuildingBlocks.Data.Essentials.Configuration.Autofac;

/// <summary>
/// Extensions to perform Autofac registrations of <see cref="DbContext"/>s 
/// </summary>
public static class RegistrationExtensions
{
    /// <summary>
    /// Registers a <see cref="DbContext"/> as a service resolvable by <typeparamref name="TRepository"/>. 
    /// This also registers an <see cref="IDbEngineConfigurator{T}"/> to be used during the context activation 
    /// </summary>
    /// <typeparam name="TContext">The <see cref="Type"/> of the <see cref="DbContext"/> to register</typeparam>
    /// <typeparam name="TRepository">The <see cref="Type"/> of the service that <typeparamref name="TContext"/> is resolvable as</typeparam>
    /// <param name="builder">The <see cref="ContainerBuilder"/></param>
    /// <param name="configurator">The <see cref="IDbEngineConfigurator{T}"/> instance to additionally register</param>
    /// <param name="contextRegistrationActions">Additional configuration actions to perform on this registration</param>
    /// <remarks>
    /// If a <see cref="DbContextOptionsBuilder{T}"/> is registered, it will be used, if not a default will be used.
    /// </remarks>
    public static void RegisterDbContext<TContext, TRepository>(
        this ContainerBuilder builder,
        IDbEngineConfigurator<TContext> configurator,
        Action<IRegistrationBuilder<TContext, ConcreteReflectionActivatorData, SingleRegistrationStyle>>? contextRegistrationActions = null)
        where TContext : BaseDbContext, TRepository
        where TRepository : IDataRepository
    {
        builder.RegisterInstance(configurator)
            .As<IDbEngineConfigurator<TContext>>();

        builder.RegisterDbContext<TContext, TRepository>(contextRegistrationActions);
    }

    /// <summary>
    /// Registers a <see cref="DbContext"/> as a service resolvable by <typeparamref name="TRepository"/>
    /// </summary>
    /// <typeparam name="TContext">The <see cref="Type"/> of the <see cref="DbContext"/> to register</typeparam>
    /// <typeparam name="TRepository">The <see cref="Type"/> of the service that <typeparamref name="TContext"/> is resolvable as</typeparam>
    /// <param name="builder">The <see cref="ContainerBuilder"/></param>
    /// <param name="contextRegistrationActions">Additional configuration actions to perform on this registration</param>
    /// <remarks>
    /// A <see cref="IDbEngineConfigurator{T}"/> is resolved during activation and needs to be separately registered. If a 
    /// <see cref="DbContextOptionsBuilder{T}"/> is registered, it will be used, if not a default will be used.
    /// </remarks>
    public static void RegisterDbContext<TContext, TRepository>(
        this ContainerBuilder builder,
        Action<IRegistrationBuilder<TContext, ConcreteReflectionActivatorData, SingleRegistrationStyle>>? contextRegistrationActions = null)
        where TContext : BaseDbContext, TRepository
        where TRepository : IDataRepository
    {
        builder.Register(c =>
        {
            var optionsBuilder = c.ResolveOptional<DbContextOptionsBuilder<TContext>>()
                ?? new DbContextOptionsBuilder<TContext>();

            var configurator = c.Resolve<IDbEngineConfigurator<TContext>>();

            configurator.Configure(optionsBuilder);

            return optionsBuilder.Options;
        })
        .As<DbContextOptions<TContext>>()
        .InstancePerLifetimeScope();

        var contextRegistration = builder.RegisterType<TContext>()
            .AsSelf()
            .As<TRepository>()
            .WithParameter(
                (pi, ctx) => pi.ParameterType == typeof(DbContextOptions),
                (pi, ctx) => ctx.Resolve<DbContextOptions<TContext>>())
            .WithParameter(
                (pi, ctx) => pi.ParameterType == typeof(BaseDbContext.ConventionsConfigurationStrategy),
                (pi, ctx) =>
                {
                    var context = ctx.Resolve<IComponentContext>();
                    return new BaseDbContext.ConventionsConfigurationStrategy(configBuilder =>
                    {
                        var configurator = context.Resolve<IDbEngineConfigurator<TContext>>();
                        configurator.ConfigureConventions(configBuilder);
                    });
                });

        contextRegistrationActions?.Invoke(contextRegistration);
    }
}
