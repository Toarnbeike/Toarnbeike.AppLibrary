using Toarnbeike.AppLibrary.Mutations;
using Toarnbeike.AppLibrary.Repositories;
using Toarnbeike.Results;
using Toarnbeike.Results.Extensions;

namespace Toarnbeike.AppLibrary.State;

/// <inheritdoc cref="IApplicationStateManager"/>
internal sealed class ApplicationStateManager(
    IAggregateStore aggregateStore, 
    ISnapshotStore snapshotStore,
    IMutationStore mutationStore) : IApplicationStateManager
{
    /// <inheritdoc />
    public bool IsInitialized { get; private set; }

    /// <inheritdoc />
    public ApplicationStatus GetStatus() => new(
            IsInitialized: IsInitialized,
            IsModified: mutationStore.CanUndo,
            HasUndoHistory: mutationStore.CanUndo,
            HasRedoHistory: mutationStore.CanRedo);

    /// <inheritdoc />
    public Result Load(string? identifier = null)
    {
        IsInitialized = false;

        var stateFile = identifier is null ? snapshotStore.GetLatestState() : snapshotStore.GetState(identifier);

        return snapshotStore
            .Read(stateFile, aggregateStore)
            .Tap(mutationStore.Clear)
            .Tap(() => IsInitialized = true);
    }

    /// <inheritdoc />
    public Result Save()
    {
        return snapshotStore
            .WriteAll(aggregateStore)
            .Tap(mutationStore.Clear);
    }
}