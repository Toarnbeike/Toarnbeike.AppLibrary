using Toarnbeike.Optional;

namespace Toarnbeike.AppLibrary.Entities.Audits;

/// <summary>
/// Soft delete information for an entity, including deletion status and details.
/// </summary>
/// <param name="IsDeleted">Indicates whether the entity is deleted.</param>
/// <param name="DeletedAt">The date and time when the entity was deleted.</param>
/// <param name="DeletedBy">The user who deleted the entity.</param>
public readonly record struct SoftDeleteInfo(
    bool IsDeleted,
    Option<string> DeletedBy,
    Option<DateTimeOffset> DeletedAt) : IValueObject
{
    /// <summary>
    /// Initializes an empty <see cref="SoftDeleteInfo"/> instance.
    /// </summary>
    /// <remarks>Use as a default or sentinel value when no soft delete data is available.</remarks>
    public static SoftDeleteInfo Empty => default;

    /// <summary>
    /// Gets a <see cref="SoftDeleteInfo"/> instance representing a not-deleted state.
    /// </summary>
    /// <remarks>Equivalent to <see cref="Empty"/>.</remarks>
    public static SoftDeleteInfo NotDeleted => Empty;
}
