using Toarnbeike.AppLibrary.Entities;
using Toarnbeike.AppLibrary.Failures;
using Toarnbeike.Optional.Extensions;
using Toarnbeike.Results;

namespace Toarnbeike.AppLibrary.Repositories;

/// <inheritdoc cref="IRepository{TAggregate, TId}"/>
/// <remarks>
/// It is implemented as a wrapper around the weakly typed <see cref="IAggregateStore"/> backend.
/// </remarks>
public class Repository<TAggregate, TId>(IAggregateStore dataStore) : IRepository<TAggregate, TId>
    where TAggregate : IAggregate<TId>
    where TId : IEntityId
{
    protected readonly IAggregateStore _dataStore = dataStore;

    /// <inheritdoc/>
    /// <remarks>Matching can be replaced with a ToResult extension method once the Toarnbeike.Functional package is available.</remarks>
    public Result<TAggregate> Get(TId id) =>
        _dataStore
            .Get<TAggregate>(id)
            .Match(Result.Success, () => new EntitiyNotFoundFailure<TAggregate>(id));

    /// <inheritdoc/>
    public IEnumerable<TAggregate> GetAll() => 
        _dataStore.GetAll<TAggregate>();
}