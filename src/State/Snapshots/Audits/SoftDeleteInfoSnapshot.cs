using Toarnbeike.AppLibrary.Entities.Audits;

namespace Toarnbeike.AppLibrary.Snapshots.Audits;

/// <summary>
/// The snapshot of audit metadata from <see cref="SoftDeleteInfo"/>
/// </summary>
/// <param name="IsDeleted">Indicates whether the entity is deleted.</param>
/// <param name="DeletedAt">The date and time when the entity was deleted, or null if not deleted.</param>
/// <param name="DeletedBy">The identifier of the user who deleted the entity, or null if not deleted.</param>
public sealed record SoftDeleteInfoSnapshot(
    bool IsDeleted,
    string? DeletedBy,
    DateTimeOffset? DeletedAt) : ISnapshot
{
    public static int SchemaVersion => 1;
}
