namespace Toarnbeike.AppLibrary.Entities.Audits;

/// <summary>
/// Represents an entity that exposes audit metadata and can produce a copy with updated audit metadata.
/// </summary>
/// <typeparam name="TSelf">The concrete auditable type that implements the interface and is returned when audit metadata is updated.</typeparam>
public interface IAuditable<out TSelf>
{
    /// <summary>
    /// Gets the audit information for the entity.
    /// </summary>
    AuditInfo AuditInfo { get; }

    /// <summary>
    /// Creates a copy of the entity with the specified audit information.
    /// </summary>
    /// <param name="auditInfo">The new audit information.</param>
    /// <returns>A new entity with the updated audit information.</returns>
    TSelf WithAuditInfo(AuditInfo auditInfo);
}
