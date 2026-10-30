namespace Toarnbeike.AppLibrary.Entities.Audits;

/// <summary>
/// Defines a contract for entities that expose soft-delete metadata and can produce a copy with updated soft-delete state.
/// </summary>
/// <remarks>Soft delete enables entities to be marked as deleted without physically removing them from the data store.</remarks>
/// <typeparam name="TSelf">The implementing entity type that supports soft deletion and is returned when updating soft-delete metadata.</typeparam>
public interface ISoftDeletable<out TSelf>
    where TSelf : ISoftDeletable<TSelf>
{
    /// <summary>
    /// Gets the soft delete information for the entity.
    /// </summary>
    SoftDeleteInfo SoftDeleteInfo { get; }

    /// <summary>
    /// Creates a copy of the entity with the specified soft delete information.
    /// </summary>
    /// <param name="softDeleteInfo">The new soft delete information.</param>
    /// <returns>A new entity with the updated soft delete information.</returns>
    TSelf WithSoftDeleteInfo(SoftDeleteInfo softDeleteInfo);
}