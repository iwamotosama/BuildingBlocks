using System.Linq.Expressions;

namespace Nikuman.BuildingBlocks.Data.Essentials;

/// <summary>
/// A repository that supports querying for and saving entities
/// </summary>
public interface IDataRepository
{
    /// <summary>
    /// Persists the changes to the repository
    /// </summary>
    /// <param name="cxlToken">Object to support cancellation</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation whose result
    /// is the number of updated records
    /// </returns>
    /// <exception cref="OperationCanceledException">The <paramref name="cxlToken"/> is cancelled</exception> 
    Task<int> SaveChangesAsync(CancellationToken cxlToken = default);

    /// <summary>
    /// Returns the first element of a sequence
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the elements  of <paramref name="query"/></typeparam>
    /// <param name="query">The <see cref="IQueryable{T}"/> to return the first element of</param>
    /// <param name="cxlToken">Object to support cancellation</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operations whose result is the
    /// first element in <paramref name="query"/>
    /// </returns>
    /// <exception cref="InvalidOperationException"><paramref name="query"/> contains no elements</exception>
    /// <exception cref="OperationCanceledException">The <paramref name="cxlToken"/> is cancelled</exception> 
    Task<T> ExecuteFirst<T>(IQueryable<T> query, CancellationToken cxlToken = default);

    /// <summary>
    /// Returns the first element of a sequence that satisfies the specified condition
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the elements  of <paramref name="query"/></typeparam>
    /// <param name="query">The <see cref="IQueryable{T}"/> to return the first element of</param>
    /// <param name="predicate">A function to test each element for a condition</param>
    /// <param name="cxlToken">Object to support cancellation</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operations whose result is the
    /// first element in <paramref name="query"/>
    /// </returns>
    /// <exception cref="InvalidOperationException"><paramref name="query"/> contains no elements</exception>
    /// <exception cref="OperationCanceledException">The <paramref name="cxlToken"/> is cancelled</exception> 
    Task<T> ExecuteFirst<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cxlToken = default);

    /// <summary>
    /// Returns the only element of a sequence
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the elements  of <paramref name="query"/></typeparam>
    /// <param name="query">The <see cref="IQueryable{T}"/> to return the first element of</param>
    /// <param name="cxlToken">Object to support cancellation</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operations whose result is the
    /// only element in <paramref name="query"/>
    /// </returns>
    /// <exception cref="InvalidOperationException"><paramref name="query"/> contains no elements or more than one elements</exception>
    /// <exception cref="OperationCanceledException">The <paramref name="cxlToken"/> is cancelled</exception> 
    Task<T> ExecuteSingle<T>(IQueryable<T> query, CancellationToken cxlToken = default);

    /// <summary>
    /// Returns the only element of a sequence that satisfies a condition
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the elements  of <paramref name="query"/></typeparam>
    /// <param name="query">The <see cref="IQueryable{T}"/> to return the first element of</param>
    /// <param name="predicate">A function to test each element for a condition</param>
    /// <param name="cxlToken">Object to support cancellation</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operations whose result is the
    /// only element in <paramref name="query"/>
    /// </returns>
    /// <exception cref="InvalidOperationException"><paramref name="query"/> contains no elements or more than one elements</exception>
    /// <exception cref="OperationCanceledException">The <paramref name="cxlToken"/> is cancelled</exception> 
    Task<T> ExecuteSingle<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cxlToken = default);

    /// <summary>
    /// Returns the first element of a sequence or default value if the sequence contains no elements
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the elements  of <paramref name="query"/></typeparam>
    /// <param name="query">The <see cref="IQueryable{T}"/> to return the first element of</param>
    /// <param name="cxlToken">Object to support cancellation</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operations whose result is the
    /// first element in <paramref name="query"/>
    /// </returns>
    /// <exception cref="OperationCanceledException">The <paramref name="cxlToken"/> is cancelled</exception> 
    Task<T?> ExecuteFirstOrDefault<T>(IQueryable<T> query, CancellationToken cxlToken = default);

    /// <summary>
    /// Returns the first element of a sequence that satisfies a specified condition or default value if no such element is found
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the elements  of <paramref name="query"/></typeparam>
    /// <param name="query">The <see cref="IQueryable{T}"/> to return the first element of</param>
    /// <param name="predicate">A function to test each element for a condition</param>
    /// <param name="cxlToken">Object to support cancellation</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operations whose result is the
    /// first element in <paramref name="query"/>
    /// </returns>
    /// <exception cref="OperationCanceledException">The <paramref name="cxlToken"/> is cancelled</exception> 
    Task<T?> ExecuteFirstOrDefault<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cxlToken = default);

    /// <summary>
    /// Returns the only element of a sequence or default value if the sequence is empty
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the elements  of <paramref name="query"/></typeparam>
    /// <param name="query">The <see cref="IQueryable{T}"/> to return the first element of</param>
    /// <param name="cxlToken">Object to support cancellation</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operations whose result is the
    /// first element in <paramref name="query"/>
    /// </returns>
    /// <exception cref="InvalidOperationException"><paramref name="query"/> contains more than one element</exception>
    /// <exception cref="OperationCanceledException">The <paramref name="cxlToken"/> is cancelled</exception> 
    Task<T?> ExecuteSingleOrDefault<T>(IQueryable<T> query, CancellationToken cxlToken = default);

    /// <summary>
    /// Returns the only element of a sequence that satisfies a specified condition or default value if no such element is found
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the elements  of <paramref name="query"/></typeparam>
    /// <param name="query">The <see cref="IQueryable{T}"/> to return the first element of</param>
    /// <param name="predicate">A function to test each element for a condition</param>
    /// <param name="cxlToken">Object to support cancellation</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operations whose result is the
    /// first element in <paramref name="query"/>
    /// </returns>
    /// <exception cref="InvalidOperationException"><paramref name="query"/> contains more than one element</exception>
    /// <exception cref="OperationCanceledException">The <paramref name="cxlToken"/> is cancelled</exception> 
    Task<T?> ExecuteSingleOrDefault<T>(IQueryable<T> query, Expression<Func<T, bool>> predicate, CancellationToken cxlToken = default);

    /// <summary>
    /// Creates a list by enumerating the <paramref name="query"/> 
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the elements of <paramref name="query"/></typeparam>
    /// <param name="query">The <see cref="IQueryable{T}"/> to enumerate</param>
    /// <param name="cxlToken">Object to support cancellation</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation whose result is a list containing the 
    /// elements from the input sequence
    /// </returns>
    /// <exception cref="OperationCanceledException">The <paramref name="cxlToken"/> is cancelled</exception> 
    Task<IReadOnlyList<T>> Execute<T>(IQueryable<T> query, CancellationToken cxlToken = default);

    /// <summary>
    /// Creates a set by enumerating the <paramref name="query"/>
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the elements of <paramref name="query"/></typeparam>
    /// <param name="query">The <see cref="IQueryable{T}"/> to enumerate</param>
    /// <param name="cxlToken">Object to support cancellation</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation whose result is a set containing the 
    /// elements from the input sequence 
    /// </returns>
    /// <exception cref="OperationCanceledException">The <paramref name="cxlToken"/> is cancelled</exception> 
    Task<IReadOnlySet<T>> ExecuteSet<T>(IQueryable<T> query, CancellationToken cxlToken = default);

    /// <summary>
    /// Creates a dictionary by enumerating the <paramref name="query"/> according to the specified <paramref name="keySelector"/>
    /// </summary>
    /// <typeparam name="TSource">The <see cref="Type"/> of the elements of <paramref name="query"/></typeparam>
    /// <typeparam name="TKey">The <see cref="Type"/> of the key returned by <paramref name="keySelector"/></typeparam>
    /// <param name="query">The <see cref="IQueryable{T}"/> to enumerate</param>
    /// <param name="keySelector">A function to extract a key from each element</param>
    /// <param name="cxlToken">Object to support cancellation</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation whose result is a dictionary that contains the
    /// selected keys and values 
    /// </returns>
    /// <exception cref="OperationCanceledException">The <paramref name="cxlToken"/> is cancelled</exception> 
    Task<IReadOnlyDictionary<TKey, TSource>> ExecuteDictionary<TSource, TKey>(IQueryable<TSource> query, Func<TSource, TKey> keySelector, CancellationToken cxlToken = default)
        where TKey : notnull;

    /// <summary>
    /// Creates a dictionary by enumerating the <paramref name="query"/> according to the specified <paramref name="keySelector"/>
    /// and <paramref name="elementSelector"/>
    /// </summary>
    /// <typeparam name="TSource">The <see cref="Type"/> of the elements of <paramref name="query"/></typeparam>
    /// <typeparam name="TKey">The <see cref="Type"/> of the key returned by <paramref name="keySelector"/></typeparam>
    /// <typeparam name="TElem">The <see cref="Type"/> of the value returned by <paramref name="elementSelector"/></typeparam> 
    /// <param name="query">The <see cref="IQueryable{T}"/> to enumerate</param>
    /// <param name="keySelector">A function to extract a key from each element</param>
    /// <param name="elementSelector">A transform function to produce a result element from each element</param>
    /// <param name="cxlToken">Object to support cancellation</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation whose result is a dictionary that contains the
    /// selected keys and values 
    /// </returns>
    /// <exception cref="OperationCanceledException">The <paramref name="cxlToken"/> is cancelled</exception> 
    Task<IReadOnlyDictionary<TKey, TElem>> ExecuteDictionary<TSource, TKey, TElem>(IQueryable<TSource> query, Func<TSource, TKey> keySelector, Func<TSource, TElem> elementSelector, CancellationToken cxlToken = default)
        where TKey : notnull;
}