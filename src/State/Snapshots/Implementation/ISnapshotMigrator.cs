namespace Toarnbeike.AppLibrary.Snapshots.Implementation;

/// <summary>
/// Snapshot migration is the process of migrating a previous snapshot version to the current snapshot version.
/// </summary>
internal interface ISnapshotMigrator
{
    /// <summary>
    /// Gets the source type.
    /// </summary>
    Type SourceType { get; }

    /// <summary>
    /// The type of the snapshot after the migration.
    /// </summary>
    Type TargetType { get; }

    /// <summary>
    /// The version of the original type.
    /// </summary>
    int SourceVersion { get; }

    /// <summary>
    /// The version of the type after migration.
    /// </summary>
    int TargetVersion { get; }

    /// <summary>
    /// The actual migration migrates a complete document of all the snapshots that have to be migrated.
    /// </summary>
    /// <param name="document">The document containing the old data that has to be migrated.</param>
    /// <returns>A document containing the new data after the migration.</returns>
    /// <remarks>Should throw an exception if the migration cannot be performed.</remarks>
    ISnapshotDocument Migrate(ISnapshotDocument document);
}
