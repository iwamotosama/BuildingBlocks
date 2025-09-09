using Nikuman.BuildingBlocks.DotNet.Essentials;
using System.Collections.Concurrent;

namespace Nikuman.BuildingBlocks.Test.Unit.DotNet.Essentials.Collections;

/// <summary>
/// Tests the functionality of <see cref="EnumerableExtensions"/> 
/// </summary>
public class EnumerableExtensionsTests
{
    /// <summary>
    /// Tests the functionality of <see cref="EnumerableExtensions.ForEach{T}(IEnumerable{T}, Action{T})"/> 
    /// </summary>
    [Fact]
    public void TestForEach()
    {
        var col = new[] { 1, 2, 3, 4, 5 };
        var counter = 0;
        var sum = 0;

        col.ForEach(i =>
        {
            counter++;
            sum += i;
        });

        // the lambda should have been run 5 times
        Assert.Equal(5, counter);

        Assert.Equal(15, sum);
    }

    /// <summary>
    /// Tests the functionality of <see cref="EnumerableExtensions.ForEach{T}(IEnumerable{T}, Func{T, Task})"/> 
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test</returns>
    [Fact]
    public async Task TestAsyncForEach()
    {
        var col = new[] { 1, 2, 3, 4, 5 };
        var bag = new ConcurrentBag<int>();

        await col.ForEach(i =>
        {
            bag.Add(i);
            return Task.CompletedTask;
        });

        // the async lambda should have run 5 times
        Assert.Equal(5, bag.Count);
        
        Assert.Equal(15, bag.Sum());
    }
}