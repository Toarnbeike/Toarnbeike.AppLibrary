using Toarnbeike.AppLibrary.Snapshots;

namespace Toarnbeike.AppLibrary.TicketApp.WorkItemTasks.Persistence;

/// <summary>
/// Snapshot of a WorkItemTask.
/// </summary>
/// <remarks>
/// Naming should include the version of the snapshot to allow for future migrations.
/// Snapshot should contain no logic, only data. It should be serializable and deserializable to/from a storage format (e.g., JSON, database record).
/// </remarks>
/// <param name="Id">The unique identifier of the work item task.</param>
/// <param name="Name">The name of the work item task.</param>
/// <param name="Description">A detailed description of the work item task.</param>
/// <param name="IsCompleted">Indicates whether the work item task is completed.</param>
public sealed record WorkItemTaskSnapshotV1(
    Guid Id,
    string Name,
    string Description,
    bool IsCompleted) : ISnapshot
{
    public static int SchemaVersion => 1;
}
