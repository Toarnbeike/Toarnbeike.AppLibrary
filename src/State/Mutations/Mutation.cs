namespace Toarnbeike.AppLibrary.Mutations;

/// <summary>
/// <inheritdoc cref="IMutation" />
/// </summary>
/// <param name="MutationDescription">A brief description of the mutation. Used for user-facing messages, such as undo/redo menu items.</param>
/// <param name="ApplyAction">Action to perform to apply the operation.</param>
/// <param name="RevertAction">Action to perform to revert the operation.</param>
internal sealed record Mutation(string MutationDescription, Action ApplyAction, Action RevertAction) : IMutation
{
    /// <inheritdoc/>
    public void Apply() => ApplyAction();

    /// <inheritdoc/>
    public void Revert() => RevertAction();
}
