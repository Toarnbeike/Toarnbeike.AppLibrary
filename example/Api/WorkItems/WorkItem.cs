using System.Collections.Immutable;
using Toarnbeike.AppLibrary.Entities;
using Toarnbeike.AppLibrary.Entities.Audits;
using Toarnbeike.AppLibrary.Results;
using Toarnbeike.AppLibrary.TicketApp.WorkItemTasks;
using Toarnbeike.AppLibrary.Transitions;
using Toarnbeike.Results;
using Toarnbeike.Results.Extensions;
using Toarnbeike.Results.Rules;
using Toarnbeike.Results.Rules.Extensions;

namespace Toarnbeike.AppLibrary.TicketApp.WorkItems;

/// <summary>
/// Represents an auditable aggregate work item with identity, name, classification, effort estimate, state, and associated tasks.
/// </summary>
/// <remarks>Serves as a base type for domain-specific work item variants.</remarks>
/// <param name="Id">The unique identifier of the work item.</param>
/// <param name="Name">The display name of the work item.</param>
/// <param name="Description">The detailed description of the work item.</param>
/// <param name="Type">The category or kind of work item.</param>
/// <param name="Effort">The estimated effort required to complete the work item.</param>
/// <param name="Status">The current workflow state of the work item.</param>
/// <param name="Tasks">The tasks associated with the work item.</param>
public abstract record WorkItem(
    WorkItemId Id, 
    string Name,
    string Description,
    string Type, 
    int? Effort, 
    WorkItemState Status, 
    ImmutableList<WorkItemTask> Tasks) : IAggregate<WorkItemId>, IAuditable<WorkItem>
{
    /// <summary>
    /// Creates a new bug work item transition in the New state.
    /// </summary>
    /// <param name="name">Bug title. Must be between 3 and 100 characters.</param>
    /// <param name="description">Bug details.</param>
    /// <param name="reproduceSteps">Steps required to reproduce the issue.</param>
    /// <param name="impact">Impact level of the bug.</param>
    /// <returns>A successful result containing the created transition for the bug as a work item when validation passes;
    /// otherwise, a failed result.</returns>
    public static Result<Transition<WorkItem>> CreateBug(string name, string description, string reproduceSteps, int impact) =>
        Result.Success()
            .Ensure(() => name).LengthBetween(3, 100)
            .Ensure(_ => impact).Positive()
            .Map(_ => new Bug(
                WorkItemId.New(), name, description, reproduceSteps, impact, null, WorkItemState.New, [])
                .Created<Bug, WorkItem>());

    /// <summary>
    /// Creates a new feature work item transition in the New state.
    /// </summary>
    /// <param name="name">Feature title, Must be between 3 and 100 characters.</param>
    /// <param name="description">Feature details.</param>
    /// <param name="requestedBy">Name of the requester.</param>
    /// <param name="businessValue">Business value score, which must be greater than zero.</param>
    /// <returns>A successful result containing the created transition for the feature as a work item when validation passes;
    /// otherwise, a failed result.</returns>
    public static Result<Transition<WorkItem>> CreateFeature(string name, string description, string requestedBy, int businessValue) =>
        Result.Success()
            .Ensure(() => name).LengthBetween(3, 100)
            .Ensure(_ => businessValue).Positive()
            .Map(_ => new Feature(
                WorkItemId.New(), name, description, requestedBy, businessValue, null, WorkItemState.New, [])
                .Created<Feature, WorkItem>());

    /// <summary>
    /// Creates a new refactoring work item transition in the New state.
    /// </summary>
    /// <param name="name">Refactoring title, Must be between 3 and 100 characters.</param>
    /// <param name="description">Refactoring details.</param>
    /// <returns>A successful result containing the created transition for the refactoring as a work item when validation passes;
    /// otherwise, a failed result.</returns>
    public static Result<Transition<WorkItem>> CreateRefactoring(string name, string description) =>
        Result.Success()
            .Ensure(() => name).LengthBetween(3, 100)
            .Map(_ => new Refactoring(
                WorkItemId.New(), name, description, null, WorkItemState.New, [])
                .Created<Refactoring, WorkItem>());

    /// <summary>
    /// Assigns an effort value and sets the work item state to Estimated when estimation is allowed.
    /// </summary>
    /// <remarks>Estimation is valid only from the New or Estimated state.</remarks>
    /// <param name="effort">Positive effort value to assign.</param>
    /// <returns>A result containing the resulting work item transition when the current state is New or Estimated, the effort is
    /// positive, and the update changes the work item; otherwise, a failed result.</returns>
    public Result<Transition<WorkItem>> Estimate(int effort) =>
        Result.Success()
            .Ensure(() => Status).OneOf([WorkItemState.New, WorkItemState.Estimated])
            .Ensure(_ => effort).Positive()
            .Map(_ => (this with { Effort = effort, Status = WorkItemState.Estimated }))
            .EnsureModified(this)
            .Map(workItem => workItem.Modified());

    /// <summary>
    /// Sets the work item to the InProgress state when the current state is New or Estimated.
    /// </summary>
    /// <returns>A successful result containing the modified work item transition when the current state is allowed; 
    /// otherwise, a failed result.</returns>
    public Result<Transition<WorkItem>> SetInProgress() =>
        Result.Success()
            .Ensure(() => Status).OneOf([WorkItemState.New, WorkItemState.Estimated])
            .Map(_ => (this with { Status = WorkItemState.InProgress }).Modified());

    /// <summary>
    /// Adds a task with the specified name and description and returns the resulting work item transition.
    /// </summary>
    /// <param name="name">The task name.</param>
    /// <param name="description">The task description.</param>
    /// <returns>A result containing a transition with the updated work item when the task is created successfully; otherwise, a failure result.</returns>
    public Result<Transition<WorkItem>> AddTask(string name, string description) =>
        WorkItemTask.Create(name, description)
            .Map(task => (this with { Tasks = Tasks.Add(task) }).Modified());

    /// <summary>
    /// Modifies the specified task with a new name and description and applies the change to the work item.
    /// </summary>
    /// <param name="taskId">Identifier of the task to update.</param>
    /// <param name="newName">New task name.</param>
    /// <param name="description">New task description.</param>
    /// <returns>A result containing the work item transition with the updated task when the update succeeds; otherwise a failure result.</returns>
    public Result<Transition<WorkItem>> ModifyTask(WorkItemTaskId taskId, string newName, string description) =>
        Tasks.GetEntity(taskId)
            .Bind(task => task.Modify(newName, description)
                .Map(modified => (this with { Tasks = Tasks.Replace(task, modified) }).Modified()));

    /// <summary>
    /// Modifies the specified task as completed.
    /// </summary>
    /// <param name="taskId">Identifier of the task to update.</param>
    /// <returns>A result containing the work item transition with the updated task when the update succeeds; otherwise a failure result.</returns>
    public Result<Transition<WorkItem>> MarkTaskAsCompleted(WorkItemTaskId taskId) =>
        Tasks.GetEntity(taskId)
            .Bind(task => task.MarkCompleted()
                .Map(modified => (this with { Tasks = Tasks.Replace(task, modified) }).Modified()));

    /// <inheritdoc/>
    public AuditInfo AuditInfo { get; private init; }

    /// <inheritdoc/>
    public WorkItem WithAuditInfo(AuditInfo auditInfo) =>
        this with { AuditInfo = auditInfo };
}