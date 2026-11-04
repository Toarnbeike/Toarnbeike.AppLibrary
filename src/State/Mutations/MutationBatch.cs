using Toarnbeike.AppLibrary.Extensions;

namespace Toarnbeike.AppLibrary.Mutations;

/// <summary>
/// A collection of mutations that can be applied and reverted as a single operation.
/// </summary>
/// <param name="mutations">The mutations to be applied and reverted as a single operation.</param>
/// <param name="batchDescription">A brief description of the batch of mutations. Used for user-facing messages, such as undo/redo menu items.</param>
internal sealed class MutationBatch(IEnumerable<IMutation> mutations, string batchDescription) : IMutation
{
    /// <inheritdoc/>
    public string MutationDescription => batchDescription;

    /// <inheritdoc/>
    public void Apply()
    {
        mutations.ForEach(mutation => mutation.Apply());
    }

    /// <inheritdoc/>
    public void Revert()
    {
        mutations.Reverse().ForEach(mutation => mutation.Revert());
    }
}