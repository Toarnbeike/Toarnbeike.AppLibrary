namespace Toarnbeike.AppLibrary.Entities.Audits;

/// <summary>
/// Extensions for the <see cref="ISoftDeletable{TSelf}"/> that simplify updating soft delete information.
/// </summary>
public static class SoftDeletableExtensions
{
    /// <typeparam name="TSelf">The type of the entity that implements <see cref="ISoftDeletable{TSelf}"/>.</typeparam>
    /// <param name="entity">The entity to update.</param>
    extension<TSelf>(TSelf entity) where TSelf : ISoftDeletable<TSelf>
    {
        /// <summary>
        /// Updates the entity with the specified soft delete information.
        /// </summary>
        /// <param name="deletedBy">The identifier of the user who deleted the entity.</param>
        /// <param name="deletedAt">The timestamp when the entity was deleted.</param>
        /// <returns>A new instance of the entity with the updated soft delete information.</returns>
        public TSelf WithSoftDeleted(string deletedBy, DateTimeOffset deletedAt) =>
            entity.WithSoftDeleteInfo(new SoftDeleteInfo(true, deletedBy, deletedAt));
    }
}