namespace Toarnbeike.AppLibrary.Snapshots.Implementation;

/// <summary>
/// A registry for a all snapshot definitions and migrators that are registred using dependency injection.
/// This is used to find the appropriate snapshot definition and migrator for a given aggregate type or resource name, as well as to find the appropriate snapshot type for a given schema version.
/// </summary>
internal sealed class SnapshotRegistry(IEnumerable<ISnapshotDefinition> definitions, IEnumerable<ISnapshotMigrator> migrators)
{
    /// <summary>
    /// Get the snapshot definition for a given aggregate type.
    /// </summary>
    /// <remarks>Will be used for serializing snapshots for the given aggregate type.</remarks>
    /// <param name="aggregateType">The type of the aggregate.</param>
    /// <returns>A snapshot definition containing the current snapshot version, type and resource name.</returns>
    public ISnapshotDefinition? GetDefinition(Type aggregateType) =>
        definitions.SingleOrDefault(x => x.AggregateType == aggregateType);

    /// <summary>
    /// Get the snapshot definition for a given resource name.
    /// </summary>
    /// <remarks>Will be used for deserializing snapshots for the given resource name.</remarks>
    /// <param name="resourceName">The name of stored resource.</param>
    /// <returns>A snapshot definition containing the current snapshot version, type and resource name.</returns>
    public ISnapshotDefinition? GetDefinition(string resourceName) =>
        definitions.SingleOrDefault(defintion => string.Equals(defintion.ResourceName, resourceName, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Get the migrator for a given source snapshot type. This is used to migrate snapshots from an older version to the current version.
    /// </summary>
    /// <param name="sourceType">The source snapshot type.</param>
    /// <returns>The migrator for the given source snapshot type, or null if it cannot be found.</returns>
    public ISnapshotMigrator? GetMigratorForSourceType(Type sourceType) =>
        migrators.SingleOrDefault(x => x.SourceType == sourceType);


#pragma warning disable S125
    ///// <summary>
    ///// Publically expose all the definitions in the registry.
    ///// </summary>
    //[Obsolete("This property is might or might not be required. I hope it is not")]
    //public IEnumerable<ISnapshotDefinition> Definitions => definitions;
#pragma warning restore S125

    //todo: Rename to GetSnapshotTypeForVersion()
    /// <summary>
    /// Find the snapshot type for the given definition and schema version. 
    /// If the schema version is less than the current schema version, it will find the appropriate snapshot type by backwards traversing the migrators.
    /// </summary>
    /// <remarks>Used to find the original version of the snapshot to deserialize incoming data to. The type is known from the resource name and the version is known from the raw data.</remarks>
    /// <param name="definition">The current snapshot defintion. Contains the current snapshot type and schema version.</param>
    /// <param name="requestedSchemaVersion">The target schema version.</param>
    /// <returns>The snapshot type for the given schema version, or null if it cannot be found.</returns>
    public Type? GetSnapshotType(ISnapshotDefinition definition, int requestedSchemaVersion)

    {
        // If the schema version is less than or equal to zero, or greater than the current schema version, return null.
        if (requestedSchemaVersion <= 0 || requestedSchemaVersion > definition.CurrentSchemaVersion)
        {
            return null;
        }

        // Keep track of the current type and corresponding schema version as we traverse the migrators backwards to find the appropriate snapshot type for the requested schema version.
        var currentType = definition.CurrentSnapshotType;
        var currentVersion = definition.CurrentSchemaVersion;

        while (currentVersion > requestedSchemaVersion)
        {
            // Check if there is a migrator for with the current type as the target type.
            var migrator = migrators.SingleOrDefault(x => x.TargetType == currentType);
            if (migrator == null)
            {
                return null;
            }

            // Update the current type and version to the source type and version of the migrator.
            currentType = migrator.SourceType;
            currentVersion = migrator.SourceVersion;
        }
        
        return currentType;
    }
}
