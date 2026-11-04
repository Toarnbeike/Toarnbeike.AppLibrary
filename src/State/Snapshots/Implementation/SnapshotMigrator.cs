namespace Toarnbeike.AppLibrary.Snapshots.Implementation;

/// <summary>
/// <para>Concrete implementation of a snapshot migrator. The interface is weakly typed, the implementation however is strongly typed using the generic type parameters.</para>
/// <inheritdoc />
/// </summary>
/// <typeparam name="TOldSnapshot">The type of the old snapshot.</typeparam>
/// <typeparam name="TNewSnapshot">The type of the new snapshot.</typeparam>
/// <param name="migrationInstruction">The user provided instructions of how to perform the migration.</param>
internal sealed class SnapshotMigrator<TOldSnapshot, TNewSnapshot>(
    ISnapshotMigration<TOldSnapshot, TNewSnapshot> migrationInstruction)
    : ISnapshotMigrator
    where TOldSnapshot : ISnapshot
    where TNewSnapshot : ISnapshot
{
    /// <inheritdoc />
    public Type SourceType => typeof(TOldSnapshot);

    /// <inheritdoc />
    public Type TargetType => typeof(TNewSnapshot);
    
    /// <inheritdoc />
    public int SourceVersion => TOldSnapshot.SchemaVersion;
    
    /// <inheritdoc />
    public int TargetVersion => TNewSnapshot.SchemaVersion;

    /// <inheritdoc />
    public ISnapshotDocument Migrate(
        ISnapshotDocument document)
    {
        var oldDocument = (SnapshotDocument<TOldSnapshot>)document;

        var snapshots = oldDocument
            .Snapshots
            .Select(migrationInstruction.Migrate)
            .ToArray();

        return new SnapshotDocument<TNewSnapshot>(oldDocument.ResourceName, snapshots);
    }
}