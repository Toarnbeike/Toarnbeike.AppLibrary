namespace Toarnbeike.AppLibrary.Snapshots;

/// <summary>
/// Instruction on how to perform a migration from one snapshot type to another.
/// </summary>
/// <remarks>
/// When introducing a new snapshot version, implement a migration from the previous version to the new version.
/// Migrations are expected to be sequential, meaning that if you have multiple versions, you should implement migrations from each version to the next, rather than skipping versions.
/// It is recommended to indicate the snapshot version in the class name of the migration, e.g. <c>MyDomainObjectSnapshotMigrationV1ToV2</c>.
/// </remarks>
/// <typeparam name="TOldSnapshot">The type of the source snapshot to migrate from.</typeparam>
/// <typeparam name="TNewSnapshot">The type of the target snapshot produced by the migration.</typeparam>
public interface ISnapshotMigration<in TOldSnapshot, out TNewSnapshot>
    where TOldSnapshot : ISnapshot
    where TNewSnapshot : ISnapshot
{
    /// <summary>
    /// Migrate the <paramref name="oldSnapshot"/> to a new snapshot of type <typeparamref name="TNewSnapshot"/>.
    /// </summary>
    /// <param name="oldSnapshot">The old snapshot to migrate.</param>
    /// <returns>The migrated new snapshot.</returns>
    TNewSnapshot Migrate(TOldSnapshot oldSnapshot);
}