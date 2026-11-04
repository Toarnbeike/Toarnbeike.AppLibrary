using System.Collections.Immutable;
using Toarnbeike.AppLibrary.Results;
using Toarnbeike.AppLibrary.TicketApp.Projects;
using Toarnbeike.AppLibrary.TicketApp.WorkItemTasks;
using Toarnbeike.AppLibrary.Transitions;
using Toarnbeike.Optional;
using Toarnbeike.Results;
using Toarnbeike.Results.Extensions;
using Toarnbeike.Results.Rules;
using Toarnbeike.Results.Rules.Extensions;

namespace Toarnbeike.AppLibrary.TicketApp.WorkItems;

/// <summary>
/// Represents a refactoring work item.
/// </summary>
/// <param name="Id">Unique identifier of the work item.</param>
/// <param name="Name">Display name of the work item.</param>
/// <param name="Description">Detailed description of the work item.</param>
/// <param name="Effort">Estimated effort to complete the work item.</param>
/// <param name="Status">Current state of the work item.</param>
/// <param name="Tasks">Tasks associated with the work item.</param>
public sealed record Refactoring(
    WorkItemId Id,
    ProjectId ProjectId,
    string Name,
    string Description,
    Option<int> Effort,
    WorkItemState Status,
    ImmutableList<WorkItemTask> Tasks)
    : WorkItem(Id, ProjectId, Name, Description, "Refactoring", Effort, Status, Tasks)
{
    /// <summary>
    /// Updates the work item name and description and produces a modification transition.
    /// </summary>
    /// <param name="name">New work item name. Must be between 3 and 100 characters.</param>
    /// <param name="description">New work item description.</param>
    /// <returns>A result containing the work item transition when the update is valid and changes were made;
    /// otherwise, a failed result.</returns>
    public Result<Transition<WorkItem>> Update(string name, string description) =>
        Result.Success()
            .Ensure(() => Status).NotOneOf([WorkItemState.Closed])
            .Ensure(_ => name).LengthBetween(3, 100)
            .Map(_ => (this with { Name = name, Description = description }))
            .EnsureModified(this)
            .Map(workItem => workItem.Modified<Refactoring, WorkItem>());
}
