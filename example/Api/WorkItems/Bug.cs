using System.Collections.Immutable;
using Toarnbeike.AppLibrary.Results;
using Toarnbeike.AppLibrary.TicketApp.WorkItemTasks;
using Toarnbeike.AppLibrary.Transitions;
using Toarnbeike.Results;
using Toarnbeike.Results.Extensions;
using Toarnbeike.Results.Rules;
using Toarnbeike.Results.Rules.Extensions;

namespace Toarnbeike.AppLibrary.TicketApp.WorkItems;

/// <summary>
/// Represents a bug work item with reproduction details and impact.
/// </summary>
/// <remarks>This record specializes WorkItem with the fixed work item type value "Bug".</remarks>
/// <param name="Id">Unique identifier of the work item.</param>
/// <param name="Name">Short title of the bug.</param>
/// <param name="Description">Detailed description of the bug.</param>
/// <param name="ReproduceSteps">Steps required to reproduce the bug.</param>
/// <param name="Impact">Severity or impact level of the bug.</param>
/// <param name="Effort">Estimated effort required to resolve the bug.</param>
/// <param name="Status">Current workflow state of the bug.</param>
/// <param name="Tasks">Tasks associated with resolving the bug.</param>
public sealed record Bug(
    WorkItemId Id, 
    string Name,
    string Description,
    string ReproduceSteps, 
    int Impact,
    int? Effort,
    WorkItemState Status, 
    ImmutableList<WorkItemTask> Tasks) 
    : WorkItem(Id, Name, Description, "Bug", Effort, Status, Tasks)
{
    /// <summary>
    /// Updates the work item name and description and produces a modification transition.
    /// </summary>
    /// <param name="name">New work item name. Must be between 3 and 100 characters.</param>
    /// <param name="description">New work item description.</param>
    /// <returns>A result containing the work item transition when the update is valid and changes were made;
    /// otherwise, a failed result.</returns>
    public Result<Transition<WorkItem>> Update(string name, string description, string reproduceSteps, int impact) =>
        Result.Success()
            .Ensure(() => Status).NotOneOf([WorkItemState.Closed])
            .Ensure(_ => name).LengthBetween(3, 100)
            .Ensure(_ => impact).Positive()
            .Map(_ => (this with { Name = name, Description = description, ReproduceSteps = reproduceSteps, Impact = impact }))
            .EnsureModified(this)
            .Map(workItem => workItem.Modified<Bug, WorkItem>());
}
