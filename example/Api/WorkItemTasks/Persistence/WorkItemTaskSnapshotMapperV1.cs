using Toarnbeike.AppLibrary.Snapshots;

namespace Toarnbeike.AppLibrary.TicketApp.WorkItemTasks.Persistence;

/// <summary>
/// Maps <see cref="WorkItemTask"/> instances to <see cref="WorkItemTaskSnapshotV1"/> snapshots and vice versa.
/// </summary>
internal sealed class WorkItemTaskSnapshotMapperV1 : ISnapshotMapper<WorkItemTask, WorkItemTaskSnapshotV1>
{
    public WorkItemTaskSnapshotV1 ToSnapshot(WorkItemTask entity) => new(
        Id: entity.Id.Value, 
        Name: entity.Name, 
        Description: entity.Description,
        IsCompleted: entity.IsCompleted);

    public WorkItemTask FromSnapshot(WorkItemTaskSnapshotV1 snapshot) => new(
        Id: new WorkItemTaskId(snapshot.Id),
        Name: snapshot.Name,
        Description: snapshot.Description,
        IsCompleted: snapshot.IsCompleted);
}