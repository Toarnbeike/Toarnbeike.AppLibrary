namespace Toarnbeike.AppLibrary.Transitions;

/// <summary>
/// The type of state transition for an item.
/// </summary>
/// <remarks>Indicates whether an item was created, modified, or deleted.</remarks>
public enum TransitionType
{
    Created,
    Modified,
    Deleted
}