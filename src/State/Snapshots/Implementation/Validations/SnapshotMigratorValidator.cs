namespace Toarnbeike.AppLibrary.Snapshots.Implementation.Validations;

/// <summary>
/// Validator that checks all snapshot migrators for
/// - duplicate source types
/// - duplicate target types
/// - invalid schema versions
/// - invalid schema version increments
/// </summary>
/// <param name="migrators"></param>
internal sealed class SnapshotMigratorValidator(IEnumerable<ISnapshotMigrator> migrators) : ISnapshotRegistryValidator
{
    /// <inheritdoc />
    public void Validate()
    {
        // Check for duplicate source types.
        var duplicateSourceTypes = migrators
            .GroupBy(m => m.SourceType)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        if (duplicateSourceTypes.Any())
        {
            throw new InvalidOperationException($"Duplicate snapshot migrators found for source types: {string.Join(", ", duplicateSourceTypes.Select(t => t.FullName))}");
        }

        // Check for duplicate target types.
        var duplicateTargetTypes = migrators
            .GroupBy(m => m.TargetType)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);
        if (duplicateTargetTypes.Any())
        {
            throw new InvalidOperationException($"Duplicate snapshot migrators found for target types: {string.Join(", ", duplicateTargetTypes.Select(t => t.FullName))}");
        }

        // Check for invalid schema versions (less than or equal to zero).
        var invalidSchemaVersions = migrators
            .Where(m => m.SourceVersion <= 0 || m.TargetVersion <= 0)
            .Select(m => $"{m.SourceType.FullName} -> {m.TargetType.FullName}");
        if (invalidSchemaVersions.Any())
        {
            throw new InvalidOperationException($"Snapshot migrators found with invalid schema versions (less than or equal to zero) for source-target type pairs: {string.Join(", ", invalidSchemaVersions)}");
        }

        // Check that each migrator only increments the schema version by 1.
        var invalidVersionIncrements = migrators
            .Where(m => m.TargetVersion != m.SourceVersion + 1)
            .Select(m => $"{m.SourceType.FullName} (v{m.SourceVersion}) -> {m.TargetType.FullName} (v{m.TargetVersion})");

        if (invalidVersionIncrements.Any())
        {
            throw new InvalidOperationException($"Snapshot migrators found with non-incremental schema versions for source-target type pairs: {string.Join(", ", invalidVersionIncrements)}");
        }
    }
}
