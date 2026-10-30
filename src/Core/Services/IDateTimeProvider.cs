namespace Toarnbeike.AppLibrary.Services;

/// <summary>
/// Provides access to the current date and time.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Gets the current date and time in UTC.
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// Gets the current local date and time as a DateTimeOffset value.
    /// </summary>
    DateTimeOffset Now { get; }

    /// <summary>
    /// Gets the current date based in UTC.
    /// </summary>
    DateOnly Today => DateOnly.FromDateTime(UtcNow);
}
