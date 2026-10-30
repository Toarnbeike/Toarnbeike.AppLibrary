namespace Toarnbeike.AppLibrary.Services;

/// <summary>
/// Provides an <see cref="ICurrentUserProvider"/> implementation that returns the current user name from the operating system environment.
/// </summary>
internal sealed class SystemUserProvider : ICurrentUserProvider
{
    public string UserName => Environment.UserName;
}
