using Toarnbeike.AppLibrary.Entities;
using Toarnbeike.AppLibrary.Entities.Audits;
using Toarnbeike.AppLibrary.Results;
using Toarnbeike.AppLibrary.TicketApp.Projects.Events;
using Toarnbeike.AppLibrary.Transitions;
using Toarnbeike.Results;
using Toarnbeike.Results.Extensions;
using Toarnbeike.Results.Rules;
using Toarnbeike.Results.Rules.Extensions;

namespace Toarnbeike.AppLibrary.TicketApp.Projects;

/// <summary>
/// Project with identity, metadata, lifecycle status, and visibility settings.
/// </summary>
/// <param name="Id">The unique identifier of the project.</param>
/// <param name="Name">The display name of the project.</param>
/// <param name="Description">A detailed description of the project.</param>
/// <param name="Status">The current lifecycle status of the project.</param>
/// <param name="Visibility">The access visibility level of the project.</param>
public sealed record Project(
    ProjectId Id,
    string Name,
    string Description,
    ProjectStatus Status,
    ProjectVisibility Visibility) : IAggregate<ProjectId>, IAuditable<Project>, ISoftDeletable<Project>
{
    /// <summary>
    /// Creates a new project in the Development status and returns its created transition.
    /// </summary>
    /// <param name="name">Project name with a length from 3 to 100 characters.</param>
    /// <param name="description">Project description that contains non-whitespace characters.</param>
    /// <param name="visibility">Visibility assigned to the project, defaults to <see cref="ProjectVisibility.Public"/>.</param>
    /// <returns>A successful result containing the created project transition when validation succeeds; otherwise, a failed result.</returns>
    public static Result<Transition<Project>> Create(string name, string description, ProjectVisibility visibility = ProjectVisibility.Public) =>
        Result.Success()
            .Ensure(() => name).LengthBetween(3, 100)
            .Ensure(_ => description).NotWhiteSpace()
            .Map(_ => new Project(
                ProjectId.New(), name, description, ProjectStatus.Development, visibility)
                .Created(project => new ProjectCreatedEvent(project.Id)));

    /// <summary>
    /// Replaces the project status with a new value when the input is valid and different from the current status.
    /// </summary>
    /// <param name="newStatus">The new status value. Must differ from the current status.</param>
    /// <returns>A successful result containing the modified project transition when replacement succeeds; otherwise, a failed result.</returns>
    public Result<Transition<Project>> ReplaceStatus(ProjectStatus newStatus) =>
        Result.Success()
            .EnsureModified(Status, newStatus)
            .Map(_ => (this with { Status = newStatus }).Modified());

    /// <summary>
    /// Updates the project name, description, and visibility and returns the resulting project transition.
    /// </summary>
    /// <param name="name">Project name, with a length from 3 to 100 characters.</param>
    /// <param name="description">Project description that cannot be null, empty, or whitespace.</param>
    /// <param name="visibility">Project visibility to apply. Defaults to public visibility.</param>
    /// <returns>A successful result containing the project transition when validation passes and at least one value changes; otherwise a failed result.</returns>
    public Result<Transition<Project>> Update(string name, string description, ProjectVisibility visibility = ProjectVisibility.Public) =>
        Result.Success()
             .Ensure(() => name).LengthBetween(3, 100)
             .Ensure(_ => description).NotWhiteSpace()
             .Map(_ => this with { Name = name, Description = description, Visibility = visibility })
             .EnsureModified(this)
             .Map(project => project.Modified());

    /// <summary>
    /// Creates a deletion transition for the project when its current status allows deletion.
    /// </summary>
    /// <remarks>Deletion is permitted only when the current status is Development or Archived.</remarks>
    /// <returns>A successful result containing a deletion transition when the status is Development or Archived; otherwise, a failed result.</returns>
    public Result<Transition<Project>> Delete() =>
        Result.Success()
            .Ensure(() => Status).OneOf([ProjectStatus.Development, ProjectStatus.Archived])
            .Map(_ => this.Deleted());

    /// <inheritdoc />
    public AuditInfo AuditInfo { get; private init; }

    /// <inheritdoc />
    public Project WithAuditInfo(AuditInfo auditInfo) =>
        this with {AuditInfo = auditInfo};

    /// <inheritdoc />
    public SoftDeleteInfo SoftDeleteInfo { get; private init; }
    
    /// <inheritdoc />
    public Project WithSoftDeleteInfo(SoftDeleteInfo softDeleteInfo) =>
        this with {SoftDeleteInfo = softDeleteInfo};
}