using System.Collections.Immutable;
using Toarnbeike.AppLibrary.Entities;
using Toarnbeike.AppLibrary.Events;

namespace Toarnbeike.AppLibrary.Transitions;

/// <summary>
/// Represents an immutable transition for an aggregate.
/// </summary>
/// <param name="Type">The type of the transition.</param>
/// <param name="Value">The value of the aggregate after it transitioned.</param>
/// <param name="Events">The domain events raised during the transition.</param>
/// <remarks>Constrained to aggregate types that implement <see cref="IAggregate"/>.</remarks>
/// <typeparam name="TAggregate">The aggregate type associated with the transition.</typeparam>
public sealed record Transition<TAggregate>(TransitionType Type, TAggregate Value, ImmutableList<IDomainEvent> Events)
    where TAggregate : IAggregate
{
    /// <summary>
    /// Creates a transition that represents the aggregate being created.
    /// </summary>
    /// <param name="value">The aggregate state for the created transition.</param>
    /// <param name="events">The domain events associated with the creation.</param>
    public static Transition<TAggregate> Created(TAggregate value, params IDomainEvent[] events) =>
        new(TransitionType.Created, value, [.. events]);

    /// <summary>
    /// Creates a transition that represents the aggregate being modified.
    /// </summary>
    /// <param name="value">The aggregate state for the modified transition.</param>
    /// <param name="events">The domain events associated with the modification.</param>
    public static Transition<TAggregate> Modified(TAggregate value, params IDomainEvent[] events) =>
        new(TransitionType.Modified, value, [.. events]);

    /// <summary>
    /// Creates a transition that represents the aggregate being deleted.
    /// </summary>
    /// <param name="value">The aggregate state for the deleted transition.</param>
    /// <param name="events">The domain events associated with the deletion.</param>
    public static Transition<TAggregate> Deleted(TAggregate value, params IDomainEvent[] events) =>
        new(TransitionType.Deleted, value, [.. events]);

    /// <summary>
    /// Creates a new transition with the current events plus the specified events appended.
    /// </summary>
    /// <remarks>Does not modify the current transition instance.</remarks>
    /// <param name="event">The first domain event to append.</param>
    /// <param name="additionalEvents">Additional domain events to append.</param>
    /// <returns>A new <see cref="Transition{TAggregate}"/> containing the existing events followed by the provided events.</returns>
    public Transition<TAggregate> AddEvents(IDomainEvent @event, params IDomainEvent[] additionalEvents) =>
        new(Type, Value, [.. Events, @event, .. additionalEvents]);
}