namespace Toarnbeike.AppLibrary.Services;

/// <summary>
/// Provides access to information about the current user.
/// </summary>
public interface ICurrentUserProvider
{
    /// <summary>
    /// Gets the user name.
    /// </summary>
    string UserName { get; }
}