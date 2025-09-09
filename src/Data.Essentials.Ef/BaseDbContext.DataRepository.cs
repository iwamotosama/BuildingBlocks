using System.Linq.Expressions;

namespace Nikuman.BuildingBlocks.Data.Essentials.Ef;

public partial class BaseDbContext : IDataRepository
{
    /// <inheritdoc/>
    protected IEntitySet<T> AdaptDbSet<T>(DbSet<T> dbSet)
        where T : class
    {
        return new DbSetAdapter<T>(dbSet);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<T>> Execute<T>(IQueryable<T> query, CancellationToken cxlToken = default)
    {
        return await query.ToArrayAsync(cxlToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyDictionary<TKey, TSource>> ExecuteDictionary<TSource, TKey>(IQueryable<TSource> query, Func<TSource, TKey> keySelector, CancellationToken cxlToken = default)
        where TKey : notnull
    {
        return await query.ToDictionaryAsync(keySelector, cxlToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyDictionary<TKey, TElem>> ExecuteDictionary<TSource, TKey, TElem>(IQueryable<TSource> query, Func<TSource, TKey> keySelector, Func<TSource, TElem> elementSelector, CancellationToken cxlToken = default)
        where TKey : notnull
    {
        return await query.ToDictionaryAsync(keySelector, elementSelector, cxlToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlySet<T>> ExecuteSet<T>(IQueryable<T> query, CancellationToken cxlToken = default)
    {
        return await query.ToHashSetAsync(cxlToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<T> ExecuteFirst<T>(IQueryable<T> query, CancellationToken cxlToken = default)
    {
        return await query.FirstAsync(cxlToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<T> ExecuteFirst<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cxlToken = default)
    {
        return await query.FirstAsync(predicate, cxlToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<T?> ExecuteFirstOrDefault<T>(IQueryable<T> query, CancellationToken cxlToken = default)
    {
        return await query.FirstOrDefaultAsync(cxlToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<T?> ExecuteFirstOrDefault<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cxlToken = default)
    {
        return await query.FirstOrDefaultAsync(predicate, cxlToken).ConfigureAwait(false);  
    }

    /// <inheritdoc/>
    public async Task<T> ExecuteSingle<T>(IQueryable<T> query, CancellationToken cxlToken = default)
    {
        return await query.SingleAsync(cxlToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<T> ExecuteSingle<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cxlToken = default)
    {
        return await query.SingleAsync(predicate, cxlToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<T?> ExecuteSingleOrDefault<T>(IQueryable<T> query, CancellationToken cxlToken = default)
    {
        return await query.SingleOrDefaultAsync(cxlToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<T?> ExecuteSingleOrDefault<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cxlToken = default)
    {
        return await query.SingleOrDefaultAsync(predicate, cxlToken).ConfigureAwait(false);
    }
}