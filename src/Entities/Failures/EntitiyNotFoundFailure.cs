using Toarnbeike.AppLibrary.Entities;
using Toarnbeike.Results.Failures;

namespace Toarnbeike.AppLibrary.Failures;

/// <summary>
/// Represents a failure when an entity of the specified type was not found.
/// </summary>
/// <typeparam name="TEntity">The type of the entity that was not found.</typeparam>
public sealed record EntitiyNotFoundFailure<TEntity> : DomainFailure
    where TEntity : IEntity
{
    /// <summary>
    /// Gets the type of the entity that was not found.
    /// </summary>
    public Type Type => typeof(TEntity);

    /// <summary>
    /// Gets a description of the value used to search for the entity.
    /// </summary>
    public string SearchDescription {get;}

    /// <summary>
    /// Initializes a new instance of the <see cref="EntitiyNotFoundFailure{TEntity}"/> class with the specified search description.
    /// </summary>
    /// <param name="searchDescription">The description of the value used to search for the entity.</param>
    /// <remarks>The search description should be used to provide context about the search criteria that led to the entity not being found.</remarks>
    public EntitiyNotFoundFailure(string searchDescription) : base($"Entity of type {typeof(TEntity)} not found") =>
        SearchDescription = searchDescription;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntitiyNotFoundFailure{TEntity}"/> class with the specified entity ID.
    /// </summary>
    /// <param name="byId">The ID of the entity that was not found.</param>
    /// <remarks>The entity ID is used to provide context about the search criteria that led to the entity not being found.</remarks>
    public EntitiyNotFoundFailure(IEntityId byId) : base($"Entity of type {typeof(TEntity)} not found") =>
        SearchDescription = $"By id '{byId}'";
}
