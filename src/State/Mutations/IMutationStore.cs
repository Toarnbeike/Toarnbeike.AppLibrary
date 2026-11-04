using Toarnbeike.Optional;
using Toarnbeike.Results;

namespace Toarnbeike.AppLibrary.Mutations;

/// <summary>
/// Store for accessing the last mutations performed in the application, allowing for undo and redo operations to be performed on them.
/// </summary>
public interface IMutationStore
{
    /// <summary>
    /// Register a mutation batch to the store.
    /// </summary>
    /// <param name="mutation">The batch to register.</param>
    void RegisterMutation(IMutation mutation);

    /// <summary>
    /// Boolean indicating if the store has any mutations that can be undone.
    /// If this is false, the undo operation should not be available, and the user should not be able to undo any mutations.
    /// </summary>
    bool CanUndo { get; }

    /// <summary>
    /// Boolean indicating if the store has any mutations that can be redone.
    /// If this is false, the redo operation should not be available, and the user should not be able to redo any mutations.
    /// </summary>
    bool CanRedo { get; }

    /// <summary>
    /// Optional description of the next mutation to undo, if any.
    /// </summary>
    Option<string> UndoDescription { get; }

    /// <summary>
    /// Optional description of the next mutation to redo, if any.
    /// </summary>
    Option<string> RedoDescription { get; }

    /// <summary>
    /// Execute the undo method of the most recent mutation in the store, effectively undoing the last mutation that was registered.
    /// </summary>
    Result Undo();

    /// <summary>
    /// Execute the redo method of the most recently undone mutation in the store, effectively redoing the last mutation that was undone.
    /// </summary>
    Result Redo();

    /// <summary>
    /// Clear all mutations from the store, effectively resetting the undo and redo stacks.
    /// Used after saving the application state, as the mutations have been persisted and are no longer able to be undone or redone.
    /// </summary>
    void Clear();
}