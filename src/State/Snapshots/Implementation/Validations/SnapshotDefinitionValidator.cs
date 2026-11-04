namespace Toarnbeike.AppLibrary.Snapshots.Implementation.Validations;

/// <summary>
/// Validator that checks all snapshot definitions for
/// - Duplicate aggregate types
/// - Duplicate resource names.
/// - Invalid schema versions.
/// </summary>
/// <param name="definitions">The snapshot definitions to validate.</param>
internal sealed class SnapshotDefinitionValidator(IEnumerable<ISnapshotDefinition> definitions) : ISnapshotRegistryValidator
{
    /// <inheritdoc />
    public void Validate()
    {
        // Check for duplicate aggregate types.
        var duplicateAggregateTypes = definitions
            .GroupBy(d => d.AggregateType)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        if (duplicateAggregateTypes.Any())
        {
            throw new InvalidOperationException($"Duplicate snapshot definitions found for aggregate types: {string.Join(", ", duplicateAggregateTypes.Select(t => t.FullName))}");
        }

        // Check for duplicate resource names.
        var duplicateResourceNames = definitions
            .GroupBy(d => d.ResourceName, StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        if (duplicateResourceNames.Any())
        {
            throw new InvalidOperationException($"Duplicate snapshot definitions found for resource names: {string.Join(", ", duplicateResourceNames)}");
        }

        // Check for invalid schema versions (less than or equal to zero).
        var invalidSchemaVersions = definitions
            .Where(d => d.CurrentSchemaVersion <= 0)
            .Select(d => d.ResourceName);

        if (invalidSchemaVersions.Any())
        {
            throw new InvalidOperationException($"Snapshot definitions found with invalid schema versions (less than or equal to zero) for resource names: {string.Join(", ", invalidSchemaVersions)}");
        }
    }
}
