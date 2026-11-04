using Toarnbeike.AppLibrary.Failures;
using Toarnbeike.Optional;
using Toarnbeike.Results;

namespace Toarnbeike.AppLibrary.Mutations;

/// <summary>
/// <para>In memory implementation of the mutation store, using two stacks to keep track of the mutations that can be undone and redone.</para>
/// <inheritdoc cref="IMutationStore"/>
/// </summary>
internal sealed class InMemoryMutationStore : IMutationStore
{
    private readonly Stack<IMutation> _undoStack = new();
    private readonly Stack<IMutation> _redoStack = new();

    /// <inheritdoc/>
    public void RegisterMutation(IMutation mutation)
    {
        _undoStack.Push(mutation);
        _redoStack.Clear();
    }

    /// <inheritdoc/>
    public bool CanUndo => _undoStack.Count > 0;
    /// <inheritdoc/>
    public bool CanRedo => _redoStack.Count > 0;

    /// <inheritdoc/>
    public Option<string> UndoDescription => CanUndo ? Option.Some(_undoStack.Peek().MutationDescription) : Option.None;
    /// <inheritdoc/>
    public Option<string> RedoDescription => CanRedo ? Option.Some(_redoStack.Peek().MutationDescription) : Option.None;

    /// <inheritdoc/>
    public Result Undo()
    {
        if (!CanUndo)
        {
            return new MethodUnavailableFailure(nameof(Undo), "No mutations to undo. Check CanUndo before calling Undo.");
        }

        var mutation = _undoStack.Pop();
        mutation.Revert();
        _redoStack.Push(mutation);
        return Result.Success();
    }

    /// <inheritdoc/>
    public Result Redo()
    {
        if (!CanRedo)
        {
            return new MethodUnavailableFailure(nameof(Redo), "No mutations to redo. Check CanRedo before calling Redo.");
        }

        var mutation = _redoStack.Pop();
        mutation.Apply();
        _undoStack.Push(mutation);
        return Result.Success();
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _undoStack.Clear();
        _redoStack.Clear();
    }
}
