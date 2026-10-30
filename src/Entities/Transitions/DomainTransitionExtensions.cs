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
        /// <param name="eventFactories">Construction factories for domain events from the newly created aggregate.</param>
        public Transition<TAggregate> Created(params Func<TAggregate, IDomainEvent>[] eventFactories) =>
            Transition<TAggregate>.Created(value, [.. eventFactories.Select(factory => factory(value))]);

        /// <summary>
        /// Creates a transition that represents the aggregate being modified.
        /// </summary>
        /// <param name="eventFactories">Construction factories for domain events from the modified version of the aggregate.</param>
        public Transition<TAggregate> Modified(params Func<TAggregate, IDomainEvent>[] eventFactories) =>
            Transition<TAggregate>.Modified(value, [.. eventFactories.Select(factory => factory(value))]);

        /// <summary>
        /// Creates a transition that represents the aggregate being deleted.
        /// </summary>
        /// <param name="eventFactories">Construction factories for domain events from the to be deleted instance of the aggregate.</param>
        public Transition<TAggregate> Deleted(params Func<TAggregate, IDomainEvent>[] eventFactories) =>
            Transition<TAggregate>.Deleted(value, [.. eventFactories.Select(factory => factory(value))]);
    }

    /// <summary>
    /// Creates a transition that represents the base of the aggregate being created.
    /// </summary>
    /// <typeparam name="TBase">The aggregate root type.</typeparam>
    /// <typeparam name="TInherit">The concrete aggregate type that inherits from TBase.</typeparam>
    /// <param name="value">The aggregate instance for the created transition.</param>
    /// <param name="eventFactories">Construction factories for domain events from the newly created aggregate.</param>
    public static Transition<TBase> Created<TInherit, TBase>(this TInherit value, params Func<TInherit, IDomainEvent>[] eventFactories)
        where TBase : IAggregate where TInherit : TBase =>
        Transition<TBase>.Created(value, [.. eventFactories.Select(factory => factory(value))]);

    /// <summary>
    /// Creates a transition that represents the base of the aggregate being modified.
    /// </summary>
    /// <typeparam name="TBase">The aggregate root type.</typeparam>
    /// <typeparam name="TInherit">The concrete aggregate type that inherits from TBase.</typeparam>
    /// <param name="value">The aggregate instance for the created transition.</param>
    /// <param name="eventFactories">Construction factories for domain events from the newly modified aggregate.</param>
    public static Transition<TBase> Modified<TInherit, TBase>(this TInherit value, params Func<TInherit, IDomainEvent>[] eventFactories)
        where TBase : IAggregate where TInherit : TBase =>
        Transition<TBase>.Modified(value, [.. eventFactories.Select(factory => factory(value))]);

    /// <summary>
    /// Creates a transition that represents the base of the aggregate being deleted.
    /// </summary>
    /// <typeparam name="TBase">The aggregate root type.</typeparam>
    /// <typeparam name="TInherit">The concrete aggregate type that inherits from TBase.</typeparam>
    /// <param name="value">The aggregate instance for the created transition.</param>
    /// <param name="eventFactories">Construction factories for domain events from the newly deleted aggregate.</param>
    public static Transition<TBase> Deleted<TInherit, TBase>(this TInherit value, params Func<TInherit, IDomainEvent>[] eventFactories)
        where TBase : IAggregate where TInherit : TBase =>
        Transition<TBase>.Deleted(value, [.. eventFactories.Select(factory => factory(value))]);
}
