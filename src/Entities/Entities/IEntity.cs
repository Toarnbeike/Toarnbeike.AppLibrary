namespace Toarnbeike.AppLibrary.Entities;

/// <summary>
/// Represents a domain entity with an identifier.
/// </summary>
/// <remarks>
/// Implementing this interface allows frameworks or libraries to access an entity's identifier
/// without knowing its strongly typed identifier type.
/// </remarks>
public interface IEntity
{
    /// <summary>
    /// Gets the entity's identifier.
    /// </summary>
    IEntityId EntityId { get; }
}

/// <summary>
/// Represents a domain entity with a strongly-typed identifier.
/// </summary>
/// <typeparam name="TId">The type of the unique identifier for the entity.</typeparam>
public interface IEntity<out TId> : IEntity where TId : IEntityId
{
    /// <summary>
    /// Strongly-typed identifier of the entity.
    /// </summary>
    TId Id { get; }

    IEntityId IEntity.EntityId => Id;
}
