using Toarnbeike.AppLibrary.Entities.Audits;

namespace Toarnbeike.AppLibrary.Snapshots.Audits;

/// <summary>
/// The snapshot of audit metadata from <see cref="AuditInfo"/>.
/// </summary>
/// <param name="CreatedBy">Identifier of the user that created the entity.</param>
/// <param name="CreatedAt">Date and time when the entity was created.</param>
/// <param name="LastModifiedBy">Identifier of the user that last modified the entity, or null when no modification has occurred.</param>
/// <param name="LastModifiedAt">Date and time of the most recent modification, or null when no modification has occurred.</param>
public sealed record AuditInfoSnapshot(
    DateTimeOffset CreatedAt,
    string CreatedBy,
    DateTimeOffset? LastModifiedAt,
    string? LastModifiedBy) : ISnapshot
{
    public static int SchemaVersion => 1;
}