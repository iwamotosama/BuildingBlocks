using Microsoft.EntityFrameworkCore;
using Nikuman.BuildingBlocks.Data.Essentials.Ef;

namespace Nikuman.BuildingBlocks.Test.Unit.Data.Essentials.Ef;

public class DbSetAdapterTests
{
    [Fact]
    public void TestAdd()
    {
        var set = Substitute.For<DbSet<TestEntity>>();
        var adapter = new DbSetAdapter<TestEntity>(set);

        var entity = new TestEntity { Id = 0 };
        adapter.Add(entity);

        set.Received(1).Add(entity);    // ensure call is passed through to the DbSet
    }

    [Fact]
    public void TestRemove()
    {
        var set = Substitute.For<DbSet<TestEntity>>();
        var adapter = new DbSetAdapter<TestEntity>(set);

        // add
        var entity = new TestEntity { Id = 0 };
        adapter.Add(entity);
        set.Received(1).Add(entity);

        // remove
        adapter.Remove(entity);
        set.Received(1).Remove(entity);     // ensure call is passed through to the DbSet
    }
    
    public class TestEntity
    {
        public int Id { get; set; }
    }
}