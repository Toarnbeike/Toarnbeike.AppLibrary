namespace Toarnbeike.AppLibrary.Services;

/// <summary>
/// Provides an <see cref="IDateTimeProvider"/> implementation that returns the current system date and time.
/// </summary>
internal sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
    DateTimeOffset IDateTimeProvider.Now => DateTimeOffset.Now;
}