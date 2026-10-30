using Toarnbeike.AppLibrary.Entities;

namespace Toarnbeike.AppLibrary.TicketApp.WorkItemTasks;

public readonly record struct WorkItemTaskId(Guid Value) : IEntityId
{
    public static WorkItemTaskId New() => new(Guid.CreateVersion7());
}
