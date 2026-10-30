namespace Toarnbeike.AppLibrary.Entities.Audits;

/// <summary>
/// Extensions for the <see cref="IAuditable{TSelf}"/> that simplify updating audit information.
/// </summary>
public static class AuditableExtensions
{
    /// <typeparam name="TSelf">The type of the entity that implements <see cref="IAuditable{TSelf}"/>.</typeparam>
    /// <param name="entity">The entity to update.</param>
    extension<TSelf>(TSelf entity) where TSelf : IAuditable<TSelf>
    {
        /// <summary>
        /// Updates the entity with the specified audit information.
        /// </summary>
        /// <param name="user">The user who created the entity.</param>
        /// <param name="created">The date and time when the entity was created.</param>
        /// <returns>A new instance of the entity with the updated audit information.</returns>
        public TSelf WithCreated(string user, DateTimeOffset created) =>
            entity.WithAuditInfo(entity.AuditInfo.WithCreated(user, created));

        /// <summary>
        /// Updates the entity with the specified last modification information.
        /// </summary>
        /// <param name="user">The user who last modified the entity.</param>
        /// <param name="lastModified">The date and time when the entity was last modified.</param>
        /// <returns>A new instance of the entity with the updated audit information.</returns>
        public TSelf WithLastModified(string user, DateTimeOffset lastModified) =>
            entity.WithAuditInfo(entity.AuditInfo.WithLastModified(user, lastModified));
    }
}