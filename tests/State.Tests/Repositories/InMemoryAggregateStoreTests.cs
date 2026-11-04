using Toarnbeike.Optional;
using Toarnbeike.Optional.Extensions;
using Toarnbeike.Optional.TestExtensions;

namespace Toarnbeike.AppLibrary.Repositories;

public class InMemoryAggregateStoreTests
{
    private readonly InMemoryAggregateStore _store = new();

    [Test]
    public void Set_ShouldAddAggregateToStore()
    {
        var aggregate = SimpleAggregate.CreateNew();

        _store.Set(aggregate.Id, aggregate.AsOption());

        var retrievedAggregate = _store.Get<SimpleAggregate>(aggregate.Id);
        retrievedAggregate.ShouldBeSome().Id.ShouldBe(aggregate.Id);
    }

    [Test]
    public void Set_ShouldUpdate_WhenAlreadyPresent()
    {
        var orginalValue = SubEntity.CreateNew("original");
        var originalAggregate = TestAggregate.CreateNew(orginalValue);

        _store.Set(originalAggregate.Id, originalAggregate.AsOption());

        var updatedValue = SubEntity.CreateNew("updated");
        var updatedAggregate = originalAggregate with { SubEntity = updatedValue };

        _store.Set(originalAggregate.Id, updatedAggregate.AsOption());

        var retrievedAggregate = _store.Get<TestAggregate>(originalAggregate.Id);
        retrievedAggregate.ShouldBeSome().SubEntity.Data.ShouldBe("updated");
    }

    [Test]
    public void Set_ShouldRemove_WhenOptionIsNone()
    {
        var aggregate = SimpleAggregate.CreateNew();
        _store.Set(aggregate.Id, aggregate.AsOption());
        _store.Set<SimpleAggregate>(aggregate.Id, Option.None);

        var retrievedAggregate = _store.Get<SimpleAggregate>(aggregate.Id);
        retrievedAggregate.ShouldBeNone();
    }

    [Test]
    public void Get_ShouldReturnNone_WhenAggregateTypeNotPresent()
    {
        var aggregateId = new SimpleAggregateId(Guid.NewGuid());
        var retrievedAggregate = _store.Get<SimpleAggregate>(aggregateId);
        retrievedAggregate.ShouldBeNone();
    }

    [Test]
    public void Get_ShouldReturnConcreteType_WhenAggregateTypeIsPresent()
    {
        var aggregate = SimpleAggregate.CreateNew();
        _store.Set(aggregate.Id, aggregate.AsOption());
        var retrievedAggregate = _store.Get<SimpleAggregate>(aggregate.Id);
        retrievedAggregate.ShouldBeSome().ShouldBeOfType<SimpleAggregate>();
    }

    [Test]
    public void GetAll_ShouldReturnCollectionOfAggregates_WhenAggregateTypeIsPresent()
    {
        var aggregate1 = SimpleAggregate.CreateNew();
        var aggregate2 = SimpleAggregate.CreateNew();
        _store.Set(aggregate1.Id, aggregate1.AsOption());
        _store.Set(aggregate2.Id, aggregate2.AsOption());

        var allAggregates = _store.GetAll<SimpleAggregate>().ToList();

        allAggregates.ShouldContain(aggregate1);
        allAggregates.ShouldContain(aggregate2);
    }

    [Test]
    public void GetAll_ShouldReturnCollectionOfConcreteType_WhenAggregateTypeIsPresent()
    {
        var aggregate1 = SimpleAggregate.CreateNew();
        var aggregate2 = SimpleAggregate.CreateNew();
        _store.Set(aggregate1.Id, aggregate1.AsOption());
        _store.Set(aggregate2.Id, aggregate2.AsOption());
        var allAggregates = _store.GetAll<SimpleAggregate>().ToList();
        allAggregates.ForEach(agg => agg.ShouldBeOfType<SimpleAggregate>());
    }

    [Test]
    public void GetAll_ShouldReturnEmptyCollection_WhenAggregateTypeNotPresent()
    {
        var allAggregates = _store.GetAll<SimpleAggregate>().ToList();
        allAggregates.ShouldBeEmpty();
    }

    [Test]
    public void SetAll_ShouldReplaceExistingAggregates()
    {
        var aggregate1 = SimpleAggregate.CreateNew();
        var aggregate2 = SimpleAggregate.CreateNew();
        _store.Set(aggregate1.Id, aggregate1.AsOption());

        // Now the store contains just aggregate1.
        // SetAll loads all aggregates, replacing the existing ones.
        _store.SetAll([aggregate2]);

        var allAggregates = _store.GetAll<SimpleAggregate>().ToList();
        allAggregates.ShouldNotContain(aggregate1);
        allAggregates.ShouldContain(aggregate2);
    }
}
