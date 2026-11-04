using Toarnbeike.AppLibrary.Entities;
using Toarnbeike.Optional;
using Toarnbeike.Optional.Extensions;

namespace Toarnbeike.AppLibrary.Repositories;

/// <summary>
/// <para>In memory implementation of the <see cref="IAggregateStore"/></para>
/// <inheritdoc cref="IAggregateStore"/>
/// </summary>
internal sealed class InMemoryAggregateStore : IAggregateStore
{
    /// <summary>
    /// In memory storage mechanism for all aggregates.
    /// It is a nested dictionary, first by AggregateType, and then by Id of the specific aggregate.
    /// The aggregates are stored as objects, and must be cast to the appropriate type when retrieved.
    /// </summary>
    private readonly Dictionary<Type, Dictionary<IEntityId, object>> _stores = [];
    
    /// <inheritdoc />
    public Option<TAggregate> Get<TAggregate>(IEntityId id)
        where TAggregate : IAggregate
    {
        return _stores.TryGetValue(typeof(TAggregate), out var aggregates) 
            && aggregates.TryGetValue(id, out var aggregate)
            ? Option.Some((TAggregate)aggregate)
            : Option.None;
    }

    /// <inheritdoc />
    public void Set<TAggregate>(IEntityId id, Option<TAggregate> aggregate)
        where TAggregate : IAggregate
    {
        if (!_stores.TryGetValue(typeof(TAggregate), out var aggregates))
        {
            aggregates = [];
            _stores[typeof(TAggregate)] = aggregates;
        }

        aggregate.Match(
            whenSome: value => aggregates[id] = value,
            whenNone: () => aggregates.Remove(id)
        );
    }

    /// <inheritdoc />
    public IEnumerable<TAggregate> GetAll<TAggregate>()
        where TAggregate : IAggregate
    {
        return _stores.TryGetValue(typeof(TAggregate), out var aggregates)
            ? aggregates.Values.Cast<TAggregate>()
            : [];
    }
    
    /// <inheritdoc />
    public void SetAll<TAggregate>(IEnumerable<TAggregate> aggregates)
        where TAggregate : IAggregate
    {
        _stores.Clear();
        _stores[typeof(TAggregate)] = aggregates.ToDictionary(aggregate => aggregate.EntityId, aggregate => (object)aggregate);
    }
}
