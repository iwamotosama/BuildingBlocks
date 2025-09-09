using System.Collections;
using System.Linq.Expressions;

namespace Nikuman.BuildingBlocks.Data.Essentials.Ef;

/// <summary>
/// Adapts a <see cref="DbSet{T}"/> to an <see cref="IEntitySet{T}"/>  
/// </summary>
internal class DbSetAdapter<TEntity>(DbSet<TEntity> dbSet) : IEntitySet<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet = dbSet;

    public Type ElementType => _dbSet.AsQueryable().ElementType;

    public Expression Expression => _dbSet.AsQueryable().Expression;

    public IQueryProvider Provider => _dbSet.AsQueryable().Provider;

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public IEnumerator<TEntity> GetEnumerator()
    {
        return _dbSet.AsEnumerable().GetEnumerator();
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}