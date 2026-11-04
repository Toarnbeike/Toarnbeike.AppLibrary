using Toarnbeike.AppLibrary.Entities;
using Toarnbeike.AppLibrary.Failures;
using Toarnbeike.Results;

namespace Toarnbeike.AppLibrary.Repositories;

/// <summary>
/// A repository for retrieving aggregates of type <typeparamref name="TAggregate"/>, with identifiers of type <typeparamref name="TId"/>.
/// </summary>
/// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
/// <typeparam name="TId">The type of the identifier of the aggregate.</typeparam>
public interface IRepository<TAggregate, in TId>
    where TAggregate : IAggregate<TId>
    where TId : IEntityId
{
    /// <summary>
    /// Retrieves an aggregate by its identifier.
    /// </summary>
    /// <param name="id">Identifier of the aggregate to retrieve.</param>
    /// <returns>A result that contains the aggregate when found; otherwise, an <see cref="EntitiyNotFoundFailure{TAggregate}"/>.</returns>
    Result<TAggregate> Get(TId id);

    /// <summary>
    /// Retrieves all aggregate instances. If none are found, an empty sequence is returned.
    /// </summary>
    /// <returns>A sequence containing all aggregate instances.</returns>
    IEnumerable<TAggregate> GetAll();
}
