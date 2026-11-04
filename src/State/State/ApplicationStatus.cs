namespace Toarnbeike.AppLibrary.State;

/// <summary>
/// Represents the current state of an application, including initialization, modification, and undo/redo availability.
/// </summary>
/// <remarks>Provides an immutable snapshot of application state flags.</remarks>
/// <param name="IsInitialized">Indicates whether the application has completed initialization.</param>
/// <param name="IsModified">Indicates whether the current state includes unsaved changes.</param>
/// <param name="HasUndoHistory">Indicates whether at least one operation is available to undo.</param>
/// <param name="HasRedoHistory">Indicates whether at least one operation is available to redo.</param>
public sealed record ApplicationStatus(
    bool IsInitialized,
    bool IsModified,
    bool HasUndoHistory,
    bool HasRedoHistory
);