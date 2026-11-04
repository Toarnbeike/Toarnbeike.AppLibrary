using System.Collections.Immutable;
using Toarnbeike.AppLibrary.Snapshots;
using Toarnbeike.AppLibrary.Snapshots.Audits;
using Toarnbeike.AppLibrary.TicketApp.WorkItemTasks.Persistence;

namespace Toarnbeike.AppLibrary.TicketApp.WorkItems.Persistence;

/// <summary>
/// Snapshot of a work item, including task snapshots and audit metadata.
/// </summary>
/// <remarks>
/// Naming should include the version of the snapshot to allow for future migrations.
/// Snapshot should contain no logic, only data. It should be serializable and deserializable to/from a storage format (e.g., JSON, database record).
/// Here is chosen to include fields from the specialized work item types (Bug, Feature, Refactoring) in the snapshot. 
/// This allows for a single snapshot type to represent all work item types, but will result in some fields being null for certain work item types (Table per Hierarchy style). 
/// An alternative approach could be to have separate snapshot types for each work item type, but this would require more complex mapping and handling of different snapshot types.
/// </remarks>
/// <param name="Id">Unique identifier of the work item snapshot.</param>
/// <param name="ProjectId">Unique identifier of the project that contains the work item.</param>
/// <param name="Name">Name of the work item.</param>
/// <param name="Description">Detailed description of the work item.</param>
/// <param name="Type">Category or classification of the work item.</param>
/// <param name="Effort">Estimated effort required to complete the work item.</param>
/// <param name="Status">Current workflow status of the work item.</param>
/// <param name="WorkItems">Task snapshots associated with the work item.</param>
/// <param name="AuditInfo">Audit metadata for creation and modification history.</param>
/// <param name="ReproduceSteps">For BUGS: Steps to reproduce the issue or behavior.</param>
/// <param name="Impact">For BUGS: Impact level of the work item.</param>
/// <param name="RequestedBy">For FEATURES: Name or identifier of the requester.</param>
/// <param name="BusinessValue">For FEATURES: Business value score of the work item.</param>
public record WorkItemSnapshotV1(
    Guid Id,
    Guid ProjectId,
    string Name,
    string Description,
    string Type,
    int? Effort,
    string Status,
    ImmutableList<WorkItemTaskSnapshotV1> WorkItems,
    AuditInfoSnapshot AuditInfo,
    string? ReproduceSteps,
    int? Impact,
    string? RequestedBy,
    int? BusinessValue) : ISnapshot
{
    public static int SchemaVersion => 1;
}