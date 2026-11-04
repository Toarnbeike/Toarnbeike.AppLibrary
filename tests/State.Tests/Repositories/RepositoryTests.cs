using Toarnbeike.AppLibrary.Failures;
using Toarnbeike.Optional;
using Toarnbeike.Optional.Extensions;
using Toarnbeike.Results.TestExtensions;

namespace Toarnbeike.AppLibrary.Repositories;

public class RepositoryTests
{
    private readonly IAggregateStore _store = Substitute.For<IAggregateStore>();
    private readonly Repository<SimpleAggregate, SimpleAggregateId> _repository;

    public RepositoryTests()
    {
        _repository = new Repository<SimpleAggregate, SimpleAggregateId>(_store);
    }

    [Test]
    public void Get_ShouldReturnAggregate_WhenPresent()
    {
        var aggregate = SimpleAggregate.CreateNew();
        _store.Get<SimpleAggregate>(aggregate.Id).Returns(aggregate.AsOption());

        var result = _repository.Get(aggregate.Id);
        
        result.ShouldBeSuccess().Id.ShouldBe(aggregate.Id);
    }

    [Test]
    public void Get_ShouldReturnFailure_WhenNotPresent()
    {
        var aggregateId = new SimpleAggregateId(Guid.NewGuid());
        _store.Get<SimpleAggregate>(aggregateId).Returns(Option.None);
        var result = _repository.Get(aggregateId);

        result.ShouldBeFailureOfType<EntitiyNotFoundFailure<SimpleAggregate>>();
    }

    [Test]
    public void GetAll_ShouldReturnAllAggregates()
    {
        var aggregate1 = SimpleAggregate.CreateNew();
        var aggregate2 = SimpleAggregate.CreateNew();
        _store.GetAll<SimpleAggregate>().Returns([aggregate1, aggregate2]);

        var result = _repository.GetAll();
        
        result.ShouldContain(aggregate1);
        result.ShouldContain(aggregate2);
    }

    [Test]
    public void GetAll_ShouldReturnEmpty_WhenNoAggregates()
    {
        _store.GetAll<SimpleAggregate>().Returns([]);

        var result = _repository.GetAll();

        result.ShouldBeEmpty();
    }
}