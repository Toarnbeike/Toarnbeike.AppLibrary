using Toarnbeike.AppLibrary.Entities;
using Toarnbeike.AppLibrary.Failures;
using Toarnbeike.Results;

namespace Toarnbeike.AppLibrary.Results;

/// <summary>
/// Provides extension methods for retrieving entities from collections by identifier and returning a result
/// that represents either the matching entity or a not-found failure.
/// </summary>
public static class GetEntityExtensions
{
    extension<TEntity>(IEnumerable<TEntity> entities) where TEntity : IEntity
    {
        /// <summary>
        /// Retrieves an entity from a collection by its identifier and returns a result representing either the entity or a not-found failure.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entities">The collection of entities to search.</param>
        /// <param name="entityId">The identifier of the entity to retrieve.</param>
        /// <returns>A result representing either the found entity or a not-found failure.</returns>
        public Result<TEntity> GetEntity(IEntityId entityId)
        {
            var entity = entities.FirstOrDefault(e => e.EntityId.Equals(entityId));
            return entity is not null
                ? Result.Success(entity)
                : new EntitiyNotFoundFailure<TEntity>(entityId);
        }

        /// <summary>
        /// Gets the first entity that matches the specified predicate.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity in the sequence.</typeparam>
        /// <param name="entities">The sequence of entities to search.</param>
        /// <param name="predicate">The condition used to select the entity.</param>
        /// <param name="predicateDescription">A description of the predicate used when no matching entity is found.</param>
        /// <returns>A successful result containing the matching entity when found; otherwise, a failure result indicating that no
        /// matching entity was found.</returns>
        public Result<TEntity> GetEntity(Func<TEntity, bool> predicate, string predicateDescription)
        {
            var entity = entities.FirstOrDefault(predicate);
            return entity is not null
                ? Result.Success(entity)
                : new EntitiyNotFoundFailure<TEntity>(predicateDescription);
        }
    }
}