using Nikuman.BuildingBlocks.DotNet.Essentials.Collections;
using System.Collections.Concurrent;

namespace Nikuman.BuildingBlocks.Test.Unit.DotNet.Essentials.Collections;

public class EnumerableExtensionsTests
{
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