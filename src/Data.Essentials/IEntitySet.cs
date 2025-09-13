using Nikuman.BuildingBlocks.DotNet.Essentials.Collections;

namespace Nikuman.BuildingBlocks.Data.Essentials;

/// <summary>
/// A set of <typeparamref name="TEntity"/>s that can be queried against
/// </summary>
/// <typeparam name="TEntity">The <see cref="Type"/> of the entity in this set</typeparam>
public interface IEntitySet<TEntity> : IQueryable<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Adds a new <typeparamref name="TEntity"/>
    /// </summary>
    /// <param name="entity">The entity to add</param>
    void Add(TEntity entity);

    /// <summary>
    /// Removes a <typeparamref name="TEntity"/>
    /// </summary>
    /// <param name="entity">The entity to remove</param>
    void Remove(TEntity entity);

    /// <summary>
    /// Adds multiple <typeparamref name="TEntity"/>s
    /// </summary>
    /// <param name="entities">The entities to add</param>
    void AddRange(IEnumerable<TEntity> entities) => entities.ForEach(Add);
}
