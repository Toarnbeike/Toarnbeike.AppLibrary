namespace Toarnbeike.AppLibrary.Events;

/// <summary>
/// Represents a domain event that captures a significant change or occurrence within the domain model.
/// From the perspective of the application, a domain event is published during the execution of a command, while the UnitOfWork is still active.
/// Failure to publish a domain event should lead to a rollback of the UnitOfWork.
/// Domain events are handled synchronously, and the application should not proceed with further operations until all domain events have been successfully published.
/// Domain events are also handled sequentially, and a domain event may cause changes that create new mutations or additional domain events.
/// </summary>
public interface IDomainEvent;