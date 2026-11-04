namespace Toarnbeike.AppLibrary.Snapshots.Implementation;

/// <summary>
/// A snapshot document is the last step of serializing the state of an aggregate.
/// It contains the resource name, snapshot type, schema version, and the collection of snapshots.
/// In the opposite direction, a snapshot document is created after deserializing the data to the latest snapshot type, and is then used to reconstruct the aggregates from the snapshots.
/// </summary>
internal interface ISnapshotDocument
{
    /// <summary>
    /// The resource name used to identify the snapshot document.
    /// This can be used as file name, database table name, or any other identifier for the snapshot document.
    /// </summary>
    string ResourceName { get; }

    /// <summary>
    /// The type of the snapshot, used to deserialize to this type.
    /// </summary>
    Type SnapshotType { get; }

    /// <summary>
    /// The latest snapshot version corresponding with this type.
    /// </summary>
    int SchemaVersion { get; }

    /// <summary>
    /// The weakly typed collection of all snapshots of this type.
    /// </summary>
    object Collection { get; }
}
