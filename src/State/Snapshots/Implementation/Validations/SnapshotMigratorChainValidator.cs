namespace Toarnbeike.AppLibrary.Snapshots.Implementation.Validations;

/// <summary>
/// Validates that registered snapshot migrators are consistent with snapshot definitions and form complete migration chains.
/// </summary>
/// <remarks>Validation fails when a migrator references an undefined source or target snapshot type, 
/// or when a snapshot definition cannot be traced through migrators back to schema version 1.</remarks>
/// <param name="definitions">Snapshot definitions that declare the current snapshot type and schema version for each aggregate.</param>
/// <param name="migrators">Snapshot migrators that define source-to-target snapshot type and version transitions.</param>
internal sealed class SnapshotMigratorChainValidator(IEnumerable<ISnapshotDefinition> definitions, IEnumerable<ISnapshotMigrator> migrators) : ISnapshotRegistryValidator
{
    /// <inheritdoc />
    public void Validate()
    {
        // Check that each migrator has a corresponding definition for its target type.
        var missingTargetDefinitions = migrators
            .Where(m => !definitions.Any(d => d.CurrentSnapshotType == m.TargetType))
            .Select(m => m.TargetType);
        if (missingTargetDefinitions.Any())
        {
            throw new InvalidOperationException($"Snapshot migrators found with missing target definitions for snapshot types: {string.Join(", ", missingTargetDefinitions.Select(t => t.FullName))}");
        }

        var checkedMigrators = new HashSet<ISnapshotMigrator>();
        // Check that each migrator has a valid chain of migrators leading to the current snapshot version.
        foreach (var definition in definitions)
        {
            var currentType = definition.CurrentSnapshotType;
            var currentVersion = definition.CurrentSchemaVersion;

            while (currentVersion > 1)
            {
                var migrator = migrators.SingleOrDefault(m => m.TargetType == currentType);
                if (migrator == null)
                {
                    break; // stop traversing if no migrator is found for the current type
                }

                currentType = migrator.SourceType;
                currentVersion = migrator.SourceVersion;
                checkedMigrators.Add(migrator);
            }
        }

        if (checkedMigrators.Count != migrators.Count())
        {
            var unusedMigrators = migrators.Except(checkedMigrators).Select(m => $"{m.SourceType.FullName} -> {m.TargetType.FullName}");
            throw new InvalidOperationException($"Snapshot migrators found that are not part of any migration chain leading to a current snapshot version: {string.Join(", ", unusedMigrators)}");
        }
    }
}