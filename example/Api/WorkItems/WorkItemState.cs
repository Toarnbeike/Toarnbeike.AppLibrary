namespace Toarnbeike.AppLibrary.TicketApp.WorkItems;

/// <summary>
/// Represents the lifecycle state of a work item.
/// </summary>
/// <remarks>Use to track progress from creation through completion and closure.</remarks>
public enum WorkItemState
{
    New,
    Estimated,
    InProgress,
    Closed
}