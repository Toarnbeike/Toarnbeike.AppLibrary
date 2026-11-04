namespace Toarnbeike.AppLibrary.Snapshots;

/// <summary>
/// Maps domain objects to snapshots and reconstructs domain objects from snapshots.
/// Failing to restore a domain object from a snapshot should throw an exception, as this indicates a bug in the mapping code.
/// Mappers are registered in the DI container, and can therefore depend on other mappers to map subtypes of the domain object or snapshot.
/// </summary>
/// <remarks>
/// It is recommended to indicate the snapshot version in the class name of the mapper, e.g. <c>MyDomainObjectSnapshotMapperV1</c>.
/// </remarks>
/// <typeparam name="TDomainObject">The domain object type to map</typeparam>
/// <typeparam name="TSnapshot">The snapshot type to map, constrained to <see cref="ISnapshot"/>.</typeparam>
public interface ISnapshotMapper<TDomainObject, TSnapshot>
    where TSnapshot : ISnapshot
{
    /// <summary>
    /// Map the <paramref name="domainObject"/> to a snapshot of type <typeparamref name="TSnapshot"/>.
    /// </summary>
    /// <param name="domainObject">The domain object to map to a snapshot.</param>
    /// <returns>A snapshot representing the state of the domain object.</returns>
    TSnapshot ToSnapshot(TDomainObject domainObject);

    /// <summary>
    /// Reconstruct the domain object from the provided <paramref name="snapshot"/> data.
    /// </summary>
    /// <param name="snapshot">The snapshot to reconstruct the domain object from.</param>
    /// <returns>The reconstructed domain object.</returns>
    TDomainObject FromSnapshot(TSnapshot snapshot);
}