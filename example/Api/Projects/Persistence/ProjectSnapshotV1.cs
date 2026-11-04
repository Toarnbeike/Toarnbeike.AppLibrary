using Toarnbeike.AppLibrary.Snapshots;
using Toarnbeike.AppLibrary.Snapshots.Audits;

namespace Toarnbeike.AppLibrary.TicketApp.Projects.Persistence;

/// <summary>
/// Snapshot of a project, including identity, metadata, lifecycle state, and audit and soft-delete information.
/// </summary>
/// <remarks>
/// Naming should include the version of the snapshot to allow for future migrations.
/// Snapshot should contain no logic, only data. It should be serializable and deserializable to/from a storage format (e.g., JSON, database record).
/// </remarks>
/// <param name="Id">The unique identifier of the project.</param>
/// <param name="Name">The project name.</param>
/// <param name="Description">The project description.</param>
/// <param name="Status">The current project status.</param>
/// <param name="Visibility">The project visibility level.</param>
/// <param name="AuditInfo">Audit metadata for creation and last modification.</param>
/// <param name="SoftDeleteInfo">Soft-delete metadata for deletion state and timestamps.</param>
public sealed record ProjectSnapshotV1(
    Guid Id,
    string Name,
    string Description,
    string Status,
    string Visibility,
    AuditInfoSnapshot AuditInfo,
    SoftDeleteInfoSnapshot SoftDeleteInfo) : ISnapshot
{
    public static int SchemaVersion => 1;
}
