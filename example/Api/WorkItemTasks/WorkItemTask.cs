using Toarnbeike.AppLibrary.Entities;
using Toarnbeike.AppLibrary.Results;
using Toarnbeike.Results;
using Toarnbeike.Results.Extensions;
using Toarnbeike.Results.Rules;
using Toarnbeike.Results.Rules.Extensions;

namespace Toarnbeike.AppLibrary.TicketApp.WorkItemTasks;

public sealed record WorkItemTask(
    WorkItemTaskId Id,
    string Name,
    string Description,
    bool IsCompleted) : IEntity<WorkItemTaskId>
{
    /// <summary>
    /// Creates a new work item task when both name and description are not empty.
    /// </summary>
    /// <param name="name">Task name. Cannot be empty.</param>
    /// <param name="description">Task description. Cannot be empty.</param>
    /// <returns>A successful result containing a new <c>WorkItemTask</c> with a generated identifier and an incomplete status
    /// when validation succeeds; otherwise, a failed result.</returns>
    public static Result<WorkItemTask> Create(string name, string description) =>
        Result.Success()
            .Ensure(() => name).NotEmpty()
            .Ensure(_ => description).NotEmpty()
            .Map(_ => new WorkItemTask(WorkItemTaskId.New(), name, description, false));

    /// <summary>
    /// Modifies the work item task's name and description if the task is not completed.
    /// </summary>
    /// <param name="name">Task name. Cannot be empty.</param>
    /// <param name="description">Task description. Cannot be empty.</param>
    /// <returns>A successful result containing the updated task when the task is not completed and both values are not empty;
    /// otherwise, a failed result.</returns>
    public Result<WorkItemTask> Modify(string name, string description) =>
        Result.Success()
            .Ensure(() => IsCompleted).IsFalse()
            .Ensure(_ => name).NotEmpty()
            .Ensure(_ => description).NotEmpty()
            .Map(_ => this with { Name = name, Description = description })
            .EnsureModified(this);

    /// <summary>
    /// Mark the task completed
    /// </summary>
    /// <returns>A successful result containing the updated task when the task was not completed already;
    /// otherwise, a failed result.</returns>
    public Result<WorkItemTask> MarkCompleted() =>
        Result.Success()
            .Ensure(() => IsCompleted).IsFalse()
            .Map(_ => this with { IsCompleted = true });
}