using Toarnbeike.AppLibrary.Events;

namespace Toarnbeike.AppLibrary.TicketApp.Projects.Events;

/// <summary>
/// A domain event raised when a project is created.
/// </summary>
/// <param name="ProjectId">The identifier of the created project.</param>
public sealed record ProjectCreatedEvent(ProjectId ProjectId) : IDomainEvent;