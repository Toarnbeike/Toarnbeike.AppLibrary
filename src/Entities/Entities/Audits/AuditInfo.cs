using Toarnbeike.Optional;

namespace Toarnbeike.AppLibrary.Entities.Audits;

/// <summary>
/// Audit information for an entity, including creation and last modification details.
/// </summary>
/// <param name="CreatedAt">The date and time when the entity was created.</param>
/// <param name="CreatedBy">The user who created the entity.</param>
/// <param name="LastModifiedAt">The date and time when the entity was last modified.</param>
/// <param name="LastModifiedBy">The user who last modified the entity.</param>
public readonly record struct AuditInfo(
    DateTimeOffset CreatedAt,
    string CreatedBy,
    Option<DateTimeOffset> LastModifiedAt,
    Option<string> LastModifiedBy) : IValueObject
{
    /// <summary>
    /// Initializes an empty <see cref="AuditInfo"/> instance.
    /// </summary>
    /// <remarks>Use as a default or sentinel value when no audit data is available.</remarks>
    public static AuditInfo Empty => default;

    /// <summary>
    /// Updates the audit information with the specified creation details.
    /// </summary>
    /// <param name="user">The user who created the entity.</param>
    /// <param name="created">The date and time when the entity was created.</param>
    /// <returns>A new instance of the audit information with the updated creation details.</returns>
    public AuditInfo WithCreated(string user, DateTimeOffset created) =>
        this with { CreatedAt = created, CreatedBy = user };

    /// <summary>
    /// Updates the audit information with the specified last modification details.
    /// </summary>
    /// <param name="user">The user who last modified the entity.</param>
    /// <param name="lastModified">The date and time when the entity was last modified.</param>
    /// <returns>A new instance of the audit information with the updated last modification details.</returns>
    public AuditInfo WithLastModified(string user, DateTimeOffset lastModified) =>
        this with { LastModifiedAt = lastModified, LastModifiedBy = user };
}