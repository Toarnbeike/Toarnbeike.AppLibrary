using Toarnbeike.AppLibrary.Mutations;
using Toarnbeike.AppLibrary.Repositories;
using Toarnbeike.Results;
using Toarnbeike.Results.Failures;
using Toarnbeike.Results.TestExtensions;

namespace Toarnbeike.AppLibrary.State;

public class ApplicationStateManagerTests
{
    private readonly IAggregateStore _aggregateStore = Substitute.For<IAggregateStore>();
    private readonly ISnapshotStore _snapshotStore = Substitute.For<ISnapshotStore>();
    private readonly IMutationStore _mutationStore = Substitute.For<IMutationStore>();
    private readonly ApplicationStateManager _stateManager;

    public ApplicationStateManagerTests()
    {
        _stateManager = new ApplicationStateManager(_aggregateStore, _snapshotStore, _mutationStore);
    }

    [Test]
    public void GetStatus_ShouldReturnApplicationStatus()
    {
        _mutationStore.CanUndo.Returns(true);
        _mutationStore.CanRedo.Returns(false);

        var status = _stateManager.GetStatus();

        status.IsInitialized.ShouldBeFalse();
        status.IsModified.ShouldBeTrue();
        status.HasUndoHistory.ShouldBeTrue();
        status.HasRedoHistory.ShouldBeFalse();
    }

    [Test]
    public void Load_ShouldUseLatestSnapshot_WhenNoIdentifierIsProvided()
    {
        _snapshotStore.GetLatestState().Returns("latest-version");
        
        _ = _stateManager.Load();

        _snapshotStore.DidNotReceive().GetState(Arg.Any<string>());
        _snapshotStore.Received(1).Read("latest-version", _aggregateStore);
    }

    [Test]
    public void Load_ShouldUseProvidedIdentifier_WhenIdentifierIsProvided()
    {
        var identifier = "earlier-identifier";
        _snapshotStore.GetState(identifier).Returns("earlier-version");

        _ = _stateManager.Load(identifier);

        _snapshotStore.DidNotReceive().GetLatestState();
        _snapshotStore.Received(1).Read("earlier-version", _aggregateStore);
    }

    [Test]
    public void Load_ShouldSetIsInitializedToTrue_AfterLoading()
    {
        _snapshotStore.GetLatestState().Returns("latest-version");
        _snapshotStore.Read("latest-version", _aggregateStore).Returns(Result.Success());
        
        var result = _stateManager.Load();

        result.ShouldBeSuccess();
        var status = _stateManager.GetStatus();
        status.IsInitialized.ShouldBeTrue();
    }

    [Test]
    public void Load_ShouldNotSetIsInitializedToTrue_IfLoadingFails()
    {
        _snapshotStore.GetLatestState().Returns("latest-version");
        _snapshotStore.Read("latest-version", _aggregateStore).Returns(Result.Success());

        _stateManager.Load(); // Sets the state to initialized = true

        _snapshotStore.GetLatestState().Returns("invalid-version");
        _snapshotStore.Read("invalid-version", _aggregateStore).Returns(new TestFailure("Read failed"));

        var result = _stateManager.Load(); // should reset the state to initialized = false

        result.ShouldBeFailureOfType<TestFailure>();
        var status = _stateManager.GetStatus();
        status.IsInitialized.ShouldBeFalse();
    }

    [Test]
    public void Load_ShouldClearMutationStore_AfterLoading()
    {
        _snapshotStore.GetLatestState().Returns("latest-version");
        _snapshotStore.Read("latest-version", _aggregateStore).Returns(Result.Success());

        var result = _stateManager.Load();
        result.ShouldBeSuccess();
        _mutationStore.Received(1).Clear();
    }

    [Test]
    public void Load_ShouldNotClearMutationStore_IfLoadingFails()
    { 
        _snapshotStore.GetLatestState().Returns("invalid-version");
        _snapshotStore.Read("invalid-version", _aggregateStore).Returns(new TestFailure("Read failed"));

        var result = _stateManager.Load();

        result.ShouldBeFailureOfType<TestFailure>();
        _mutationStore.DidNotReceive().Clear();
    }

    [Test]
    public void Save_ShouldCallWriteAll_OnSnapshotStore()
    {
        _snapshotStore.WriteAll(_aggregateStore).Returns(Result.Success());
        
        var result = _stateManager.Save();

        result.ShouldBeSuccess();
        _snapshotStore.Received(1).WriteAll(_aggregateStore);
    }

    [Test]
    public void Save_ShouldCallClear_OnMutationStore_WhenSaveSucceeds()
    {
        _snapshotStore.WriteAll(_aggregateStore).Returns(Result.Success());

        var result = _stateManager.Save();

        result.ShouldBeSuccess();
        _mutationStore.Received(1).Clear();
    }

    [Test]
    public void Save_ShouldNotCallClear_OnMutationStore_WhenSaveFails()
    {
        _snapshotStore.WriteAll(_aggregateStore).Returns(new TestFailure("Write failed"));

        var result = _stateManager.Save();

        result.ShouldBeFailureOfType<TestFailure>();
        _mutationStore.DidNotReceive().Clear();
    }
}