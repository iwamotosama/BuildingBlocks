namespace Nikuman.BuildingBlocks.DotNet.Essentials.Collections;

/// <summary>
/// Extensions to <see cref="IEnumerable{T}"/> 
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Performs the given <paramref name="action"/> on every element in a collection
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the element in the collection</typeparam>
    /// <param name="col">The collection to iterate over</param>
    /// <param name="action">The <see cref="Action{T}"/> to perform for each element</param>
    public static void ForEach<T>(this IEnumerable<T> col, Action<T> action)
    {
        foreach (var elem in col)
        {
            action(elem);
        }
    }

    /// <summary>
    /// Performs the given asynchronous action on every element in a collection
    /// </summary>
    /// <typeparam name="T">The <see cref="Type" /> of the element in the collection</typeparam>
    /// <param name="col">The collection</param>
    /// <param name="asyncAction">The asynchronous action to perform on each element</param>
    /// <returns></returns>
    public static Task ForEach<T>(this IEnumerable<T> col, Func<T, Task> asyncAction)
    {
        return Task.WhenAll(col.Select(elem => asyncAction(elem)));
    }

    /// <summary>
    /// Returns a single-element <see cref="IEnumerable{T}"/> 
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the element in the sequence</typeparam>
    /// <param name="obj">The element to be included in the returned sequence</param>
    /// <returns>An <see cref="IEnumerable{T}"/> with <paramref name="obj"/> as its only element</returns>
    public static IEnumerable<T> One<T>(this T obj)
    {
        return Enumerable.Repeat(obj, 1);
    }
}