namespace Toarnbeike.AppLibrary.Mutations;

/// <summary>
/// A mutation describes a reversible change in state.
/// </summary>
public interface IMutation
{
    /// <summary>
    /// A brief description of the mutation. Used for user-facing messages, such as undo/redo menu items.
    /// </summary>
    string MutationDescription { get; }
    
    /// <summary>
    /// Apply the mutation.
    /// </summary>
    void Apply();

    /// <summary>
    /// Revert the mutation.
    /// </summary>
    void Revert();
}