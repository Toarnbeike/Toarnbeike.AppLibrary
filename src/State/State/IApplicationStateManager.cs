using Toarnbeike.Results;

namespace Toarnbeike.AppLibrary.State;

/// <summary>
/// Manager for the application state, providing methods to load and save the state, as well as properties to check if the state is initialized or modified.
/// </summary>
public interface IApplicationStateManager
{
    bool IsInitialized { get; }

    /// <summary>
    /// Gets the current status of the application
    /// </summary>
    ApplicationStatus GetStatus();

    /// <summary>
    /// Loads the persisted application state, replacing the current state.
    /// </summary>
    Result Load(string? identifier = null);

    /// <summary>
    /// Saves the current application state, persisting it to the underlying storage.
    /// On successful completion, the application state is considered unmodified and the
    /// mutation history is cleared, making the current state the new undo boundary.
    /// </summary>
    Result Save();
}