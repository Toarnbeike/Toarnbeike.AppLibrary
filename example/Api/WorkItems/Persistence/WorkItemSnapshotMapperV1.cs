using Toarnbeike.AppLibrary.Entities.Audits;
using Toarnbeike.AppLibrary.Snapshots;
using Toarnbeike.AppLibrary.Snapshots.Audits;
using Toarnbeike.AppLibrary.TicketApp.Projects;
using Toarnbeike.AppLibrary.TicketApp.WorkItemTasks;
using Toarnbeike.Optional.Extensions;

namespace Toarnbeike.AppLibrary.TicketApp.WorkItems.Persistence;

/// <summary>
/// Maps <see cref="WorkItem"/> instances to <see cref="WorkItemSnapshotV1"/> snapshots and vice versa.
/// </summary>
/// <remarks>
/// - Here also the version number is included in the class name to allow for future migrations and versioning of snapshot mappings.
/// - The auditInfo data is stored as snapshots. 
///   This allows for easy mapping, but might be less suitable for sql table style persistence.
///   If a more relational approach is desired, consider storing audit info as flattened fields in the snapshot.
/// - The enum values for Status are stored as strings in the snapshot.
///   This allows for future changes to the enum definitions without breaking existing snapshots.
///   An alternative approach with integer values could be used, but would require careful management of enum value changes.
/// </remarks>
/// <param name="taskMapper">Maps <see cref="WorkItemTask"/> to <see cref="WorkItemTaskSnapshotV1"/>.</param>
/// <param name="auditInfoMapper">Maps <see cref="AuditInfo"/> to <see cref="AuditInfoSnapshot"/>.</param>
internal sealed class WorkItemSnapshotMapperV1(
    ISnapshotMapper<WorkItemTask, WorkItemTaskSnapshotV1> taskMapper,
    ISnapshotMapper<AuditInfo, AuditInfoSnapshot> auditInfoMapper)
    : ISnapshotMapper<WorkItem, WorkItemSnapshotV1>
{
    public WorkItemSnapshotV1 ToSnapshot(WorkItem domainObject) => new WorkItemSnapshotV1(
        Id: domainObject.Id.Value,
        ProjectId: domainObject.ProjectId.Value,
        Name: domainObject.Name,
        Description: domainObject.Description,
        Type: domainObject.Type,
        Effort: domainObject.Effort.AsNullableValue(),
        Status: domainObject.Status.ToString(),
        WorkItems: [.. domainObject.Tasks.Select(taskMapper.ToSnapshot)],
        AuditInfo: auditInfoMapper.ToSnapshot(domainObject.AuditInfo),
        ReproduceSteps: (domainObject is Bug bug) ? bug.ReproduceSteps : null,
        Impact: (domainObject is Bug bug2) ? bug2.Impact : null,
        RequestedBy: (domainObject is Feature feature) ? feature.RequestedBy : null,
        BusinessValue: (domainObject is Feature feature2) ? feature2.BusinessValue : null
    );

    public WorkItem FromSnapshot(WorkItemSnapshotV1 snapshot)
    {
        WorkItem concreteType = snapshot.Type switch
        {
            "Bug" => new Bug(
                Id: new WorkItemId(snapshot.Id),
                ProjectId: new ProjectId(snapshot.ProjectId),
                Name: snapshot.Name,
                Description: snapshot.Description,
                ReproduceSteps: snapshot.ReproduceSteps ?? string.Empty,
                Impact: snapshot.Impact ?? 0,
                Effort: snapshot.Effort.AsOption(),
                Status: Enum.Parse<WorkItemState>(snapshot.Status),
                Tasks: [.. snapshot.WorkItems.Select(taskMapper.FromSnapshot)]
            ),
            "Feature" => new Feature(
                Id: new WorkItemId(snapshot.Id),
                ProjectId: new ProjectId(snapshot.ProjectId),
                Name: snapshot.Name,
                Description: snapshot.Description,
                RequestedBy: snapshot.RequestedBy ?? string.Empty,
                BusinessValue: snapshot.BusinessValue ?? 0,
                Effort: snapshot.Effort.AsOption(),
                Status: Enum.Parse<WorkItemState>(snapshot.Status),
                Tasks: [.. snapshot.WorkItems.Select(taskMapper.FromSnapshot)]
            ),
            "Refactoring" => new Refactoring(
                Id: new WorkItemId(snapshot.Id),
                ProjectId: new ProjectId(snapshot.ProjectId),
                Name: snapshot.Name,
                Description: snapshot.Description,
                Effort: snapshot.Effort.AsOption(),
                Status: Enum.Parse<WorkItemState>(snapshot.Status),
                Tasks: [.. snapshot.WorkItems.Select(taskMapper.FromSnapshot)]
            ),
            _ => throw new InvalidOperationException($"Unknown work item type '{snapshot.Type}' in snapshot.")
        };

        return concreteType
            .WithAuditInfo(auditInfoMapper.FromSnapshot(snapshot.AuditInfo));
    }
}