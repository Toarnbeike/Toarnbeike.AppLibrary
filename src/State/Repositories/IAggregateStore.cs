using Toarnbeike.AppLibrary.Entities;
using Toarnbeike.Optional;

namespace Toarnbeike.AppLibrary.Repositories;

/// <summary>
/// The IAggregateStore is the weakly typed backend of the repository.
/// It is responsible for generally storing and retrieving aggregates.
/// </summary>
public interface IAggregateStore
{
    /// <summary>
    /// Try to get an aggregate from the store by its id.
    /// </summary>
    /// <typeparam name="TAggregate">The type of the aggregate to retrieve</typeparam>
    /// <param name="id">The id of the aggregate to retrieve</param>
    /// <returns>The aggregate if found, otherwise None</returns>
    Option<TAggregate> Get<TAggregate>(IEntityId id)
        where TAggregate : IAggregate;

    /// <summary>
    /// Sets an aggregate in the store by its id.
    /// If the aggregate is None, it will remove the value from the store.
    /// </summary>
    /// <typeparam name="TAggregate">The type of the aggregate to set</typeparam>
    /// <param name="id">The id of the aggregate to set</param>
    /// <param name="aggregate">The aggregate to set</param>
    void Set<TAggregate>(IEntityId id, Option<TAggregate> aggregate)
        where TAggregate : IAggregate;

    /// <summary>
    /// Gets all aggregates of a specific type from the store.
    /// </summary>
    /// <typeparam name="TAggregate">The type of the aggregates to retrieve</typeparam>
    /// <returns>An enumerable containing all aggregates of the specified type</returns>
    IEnumerable<TAggregate> GetAll<TAggregate>()
        where TAggregate : IAggregate;

    /// <summary>
    /// Sets all aggregates of a specific type in the store.
    /// </summary>
    /// <typeparam name="TAggregate">The type of the aggregates to set</typeparam>
    /// <param name="aggregates">The aggregates to set</param>
    void SetAll<TAggregate>(
        IEnumerable<TAggregate> aggregates)
        where TAggregate : IAggregate;
}