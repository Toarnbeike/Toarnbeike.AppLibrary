namespace Toarnbeike.AppLibrary.Snapshots.Implementation;

/// <summary>
/// <inheritdoc cref="ISnapshotDocument"/>
/// </summary>
/// <typeparam name="TSnapshot">The type of the snapshot.</typeparam>
/// <param name="ResourceName">The name of the resource.</param>
/// <param name="Snapshots">The collection of snapshots.</param>
internal sealed record SnapshotDocument<TSnapshot>(
    string ResourceName,
    TSnapshot[] Snapshots)
    : ISnapshotDocument
    where TSnapshot : ISnapshot
{
    /// <inheritdoc/>
    public Type SnapshotType => typeof(TSnapshot);
    
    /// <inheritdoc/>
    public int SchemaVersion => TSnapshot.SchemaVersion;
    
    /// <inheritdoc/>
    object ISnapshotDocument.Collection => Snapshots;
}
