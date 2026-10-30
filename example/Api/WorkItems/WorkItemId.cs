using Toarnbeike.AppLibrary.Entities;

namespace Toarnbeike.AppLibrary.TicketApp.WorkItems;

public readonly record struct WorkItemId(Guid Value) : IEntityId
{
    public static WorkItemId New() => new(Guid.CreateVersion7());
}