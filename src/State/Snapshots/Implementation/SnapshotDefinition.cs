using Toarnbeike.AppLibrary.Entities;
using Toarnbeike.AppLibrary.Repositories;

namespace Toarnbeike.AppLibrary.Snapshots.Implementation;

/// <inheritdoc cref="ISnapshotDefinition" />
/// <typeparam name="TAggregate">The aggregate type of this definition</typeparam>
/// <typeparam name="TSnapshot">The snapshot type to used to map this aggregate to.</typeparam>
/// <param name="mapper">Mapper used to map between the aggregate and snapshot types.</param>
/// <param name="resourceName">Resource name used to identify the snapshot document.</param>
internal sealed class SnapshotDefinition<TAggregate, TSnapshot>(
    string resourceName,
    ISnapshotMapper<TAggregate, TSnapshot> mapper)
    : ISnapshotDefinition
    where TSnapshot : ISnapshot
    where TAggregate : IAggregate
{
    /// <inheritdoc />
    public Type AggregateType => typeof(TAggregate);

    /// <inheritdoc />
    public Type CurrentSnapshotType => typeof(TSnapshot);
    
    /// <inheritdoc />
    public int CurrentSchemaVersion => TSnapshot.SchemaVersion;
    
    /// <inheritdoc />
    public string ResourceName => resourceName;

    /// <inheritdoc />
    public ISnapshotDocument CreateSnapshot(IAggregateStore dataStore)
    {
        var snapshots = dataStore
            .GetAll<TAggregate>()
            .Select(mapper.ToSnapshot)
            .ToArray();

        return new SnapshotDocument<TSnapshot>(resourceName, snapshots);
    }

    /// <inheritdoc />
    public void LoadSnapshots(IAggregateStore dataStore, ISnapshotDocument document)
    {
        var typedDocument = (SnapshotDocument<TSnapshot>)document;

        var aggregates = typedDocument
            .Snapshots
            .Select(mapper.FromSnapshot)
            .ToArray();

        dataStore.SetAll(aggregates);
    }
}
