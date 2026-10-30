namespace Toarnbeike.AppLibrary.Entities;

/// <summary>
/// Represents a marker interface used to denote aggregate roots.
/// </summary>
/// <remarks>
/// This interface does not define any members and is intended to be used as a tagging mechanism.
/// Implementing this interface allows frameworks or libraries to recognize aggregate roots for purposes such as
/// persistence, validation, or event sourcing, without requiring strongly typed Id <see cref="IEntityId"/> info.
/// </remarks>
public interface IAggregate : IEntity;

/// <summary>
/// Represents an aggregate root entity in a domain-driven design context, which serves as the entry point
/// for modifying and accessing the aggregate's state.
/// </summary>
/// <remarks>
/// Aggregate roots are responsible for maintaining the consistency of changes within the aggregate. 
/// This interface is used to enforce aggregate boundaries and event tracking in domain models.
/// </remarks>
/// <typeparam name="TId">The type of the unique identifier for the aggregate root.</typeparam>
public interface IAggregate<out TId> : IEntity<TId>, IAggregate
    where TId : IEntityId;