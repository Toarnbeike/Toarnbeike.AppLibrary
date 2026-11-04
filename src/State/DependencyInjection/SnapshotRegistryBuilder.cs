using Toarnbeike.AppLibrary.Extensions;
using Toarnbeike.AppLibrary.Snapshots.Implementation;
using Toarnbeike.AppLibrary.Snapshots.Implementation.Validations;

namespace Toarnbeike.AppLibrary.DependencyInjection;

/// <summary>
/// Internal builder for the snapshot registry.
/// Requests all <see cref="ISnapshotDefinition"/> and <see cref="ISnapshotMigrator"/> from the dependency injection container and validates and builds a <see cref="SnapshotRegistry"/>.
/// </summary>
internal sealed class SnapshotRegistryBuilder(IEnumerable<ISnapshotRegistryValidator> validators, IEnumerable<ISnapshotDefinition> definitions, IEnumerable<ISnapshotMigrator> migrators)
{
    /// <summary>
    /// Builds a <see cref="SnapshotRegistry"/> from the given definitions and migrators.
    /// </summary>
    /// <returns>A <see cref="SnapshotRegistry"/> containing all the given definitions and migrators.</returns>
    public SnapshotRegistry Build()
    {
        validators.ForEach(validator => validator.Validate());
        return new SnapshotRegistry(definitions, migrators);
    }
}
