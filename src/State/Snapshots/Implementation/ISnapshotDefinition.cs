using Toarnbeike.AppLibrary.Repositories;

namespace Toarnbeike.AppLibrary.Snapshots.Implementation;

/// <summary>
/// Defines metadata and operations for creating and loading aggregate snapshots from a data store.
/// </summary>
/// <remarks>Provides the aggregate type, current snapshot type, schema version, and resource name used to identify and evolve snapshot documents.</remarks>
internal interface ISnapshotDefinition
{
    /// <summary>
    /// The type of the aggregate
    /// </summary>
    Type AggregateType { get; }

    /// <summary>
    /// The type of the latest snapshot definition for the aggregate.
    /// </summary>
    Type CurrentSnapshotType { get; }

    /// <summary>
    /// The schema version corresponding with the <see cref="CurrentSnapshotType"/>
    /// </summary>
    int CurrentSchemaVersion { get; }

    /// <summary>
    /// The resource name used to identify the snapshot document.
    /// This can be used as file name, database table name, or any other identifier for the snapshot document.
    /// </summary>
    string ResourceName { get; }

    /// <summary>
    /// Creates a snapshot document from the data for all aggregates of this type in the <see cref="IAggregateStore"/>.
    /// </summary>
    /// <param name="dataStore">Data store used as the source for the snapshot.</param>
    /// <returns>Snapshot document containing snapshots of all aggregates of this type.</returns>
    ISnapshotDocument CreateSnapshot(IAggregateStore dataStore);

    /// <summary>
    /// Load a snapshot document containing the current elements and update the data store with this data.
    /// </summary>
    /// <param name="dataStore">Data store to be updated with the snapshot data.</param>
    /// <param name="document">Snapshot document containing the data to load into the data store.</param>
    void LoadSnapshots(IAggregateStore dataStore, ISnapshotDocument document);
}