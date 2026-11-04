namespace Toarnbeike.AppLibrary.Snapshots;

/// <summary>
/// A snapshot is a serializable version of an aggregate.
/// </summary>
/// <remarks>
/// Keep the snapshot consisting only of primitive types, and other snapshot types. 
/// For all mapping code, including that of subtypes, use the <see cref="ISnapshotMapper{TAggregate, TSnapshot}"/> interface.
/// This ensures that when migrating to a new snapshot version, no mapping code is missed, and that all mapping code is in one place.
/// </remarks>
public interface ISnapshot
{
    /// <summary>
    /// Static version information for this snapshot.
    /// Used for migrations.
    /// </summary>
    static abstract int SchemaVersion { get; }
}
