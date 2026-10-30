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
/// Represents a feature work item with business value, requester, state, and associated tasks.
/// </summary>
/// <param name="Id">Unique identifier of the feature.</param>
/// <param name="Name">Title of the feature.</param>
/// <param name="Description">Detailed description of the feature.</param>
/// <param name="Effort">Estimated effort to complete the feature, or <see langword="null"/> when no estimate is available.</param>
/// <param name="RequestedBy">Name of the person or stakeholder who requested the feature.</param>
/// <param name="BusinessValue">Relative business value used for prioritization.</param>
/// <param name="Status">Current workflow state of the feature.</param>
/// <param name="Tasks">Tasks required to implement the feature.</param>
public sealed record Feature(
    WorkItemId Id,
    string Name,
    string Description,
    string RequestedBy,
    int BusinessValue,
    int? Effort,
    WorkItemState Status,
    ImmutableList<WorkItemTask> Tasks)
    : WorkItem(Id, Name, Description, "Feature", Effort, Status, Tasks)
{
    /// <summary>
    /// Updates the work item name and description and produces a modification transition.
    /// </summary>
    /// <param name="name">New work item name. Must be between 3 and 100 characters.</param>
    /// <param name="description">New work item description.</param>
    /// <param name="requestedBy">New requester name.</param>
    /// <param name="businessValue">New business value, which must be greater than zero.</param>
    /// <returns>A result containing the work item transition when the update is valid and changes were made;
    /// otherwise, a failed result.</returns>
    public Result<Transition<WorkItem>> Update(string name, string description, string requestedBy, int businessValue) =>
        Result.Success()
            .Ensure(() => Status).NotOneOf([WorkItemState.Closed])
            .Ensure(_ => name).LengthBetween(3, 100)
            .Ensure(_ => businessValue).Positive()
            .Map(_ => (this with { Name = name, Description = description, RequestedBy = requestedBy, BusinessValue = businessValue }))
            .EnsureModified(this)
            .Map(workItem => workItem.Modified<Feature, WorkItem>());
}
