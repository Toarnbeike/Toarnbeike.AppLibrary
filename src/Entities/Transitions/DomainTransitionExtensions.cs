using Toarnbeike.AppLibrary.Entities;
using Toarnbeike.AppLibrary.Events;

namespace Toarnbeike.AppLibrary.Transitions;

/// <summary>
/// Extension methods to simplify the creation of <see cref="Transition{TAggregate}"/> from aggregate instances.
/// </summary>
public static class DomainTransitionExtensions
{
    /// <typeparam name="TAggregate">The type of the aggregate that transitioned.</typeparam>
    /// <param name="value">The value of the aggregate after the transition.</param>
    extension<TAggregate>(TAggregate value) where TAggregate : IAggregate
    {
        /// <summary>
        /// Creates a transition that represents the aggregate being created.
        /// </summary>
        /// <param name="events">The domain events associated with the creation.</param>
        public Transition<TAggregate> Created(params IDomainEvent[] events) =>
            Transition<TAggregate>.Created(value, events);

        /// <summary>
        /// Creates a transition that represents the aggregate being modified.
        /// </summary>
        /// <param name="events">The domain events associated with the modification.</param>
        public Transition<TAggregate> Modified(params IDomainEvent[] events) =>
            Transition<TAggregate>.Modified(value, events);

        /// <summary>
        /// Creates a transition that represents the aggregate being deleted.
        /// </summary>
        /// <param name="events">The domain events associated with the deletion.</param>
        public Transition<TAggregate> Deleted(params IDomainEvent[] events) =>
            Transition<TAggregate>.Deleted(value, events);
    }
}
