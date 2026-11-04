using Toarnbeike.AppLibrary.Failures;
using Toarnbeike.Optional.TestExtensions;
using Toarnbeike.Results.TestExtensions;

namespace Toarnbeike.AppLibrary.Mutations;

public class InMemoryMutationStoreTests
{
    private readonly InMemoryMutationStore _store = new();

    [Test]
    public void AddMutation_AddsMutationToStore()
    {
        var mutation = new Mutation("Test", () => { }, () => { });

        _store.RegisterMutation(mutation);

        _store.CanUndo.ShouldBeTrue();
        _store.UndoDescription.ShouldBeSome("Test");
        _store.CanRedo.ShouldBeFalse();
        _store.RedoDescription.ShouldBeNone();
    }

    [Test]
    public void AddMutation_ClearsRedoStack()
    {
        var mutation1 = new Mutation("Test1", () => { }, () => { });
        var mutation2 = new Mutation("Test2", () => { }, () => { });

        _store.RegisterMutation(mutation1);
        _store.Undo();
        _store.RegisterMutation(mutation2);

        _store.CanRedo.ShouldBeFalse();
    }

    [Test]
    public void Undo_ShouldReturn_MethodUnavailableFailure_WhenNoMutationsToUndo()
    {
        var failure = _store.Undo().ShouldBeFailureOfType<MethodUnavailableFailure>();
        failure.MethodName.ShouldBe(nameof(_store.Undo));
        failure.Reason.ShouldBe("No mutations to undo. Check CanUndo before calling Undo.");
    }

    [Test]
    public void Undo_Should_RevertLastMutation()
    {
        var mutationApplied = false;
        var mutationReverted = false;
        var mutation = new Mutation("Test",
            () => mutationApplied = true,
            () => mutationReverted = true);

        _store.RegisterMutation(mutation);
        var result = _store.Undo();

        result.ShouldBeSuccess();
        mutationApplied.ShouldBeFalse();
        mutationReverted.ShouldBeTrue();
    }

    [Test]
    public void Undo_Should_PushMutationToRedoStack()
    {
        var mutation = new Mutation("Test", () => { }, () => { });
        _store.RegisterMutation(mutation);

        _store.Undo();

        _store.CanRedo.ShouldBeTrue();
        _store.RedoDescription.ShouldBeSome("Test");

        _store.CanUndo.ShouldBeFalse();
        _store.UndoDescription.ShouldBeNone();
    }

    [Test]
    public void Redo_ShouldReturn_MethodUnavailableFailure_WhenNoMutationsToRedo()
    {
        var failure = _store.Redo().ShouldBeFailureOfType<MethodUnavailableFailure>();

        failure.MethodName.ShouldBe(nameof(_store.Redo));
        failure.Reason.ShouldBe("No mutations to redo. Check CanRedo before calling Redo.");
    }

    [Test]
    public void Redo_Should_ApplyLastUndoneMutation()
    {
        var mutationApplied = false;
        var mutationReverted = false;
        var mutation = new Mutation("Test",
            () => mutationApplied = true,
            () => mutationReverted = true);

        _store.RegisterMutation(mutation);

        _store.Undo();
        var result = _store.Redo();

        result.ShouldBeSuccess();
        mutationApplied.ShouldBeTrue();
        mutationReverted.ShouldBeTrue();
    }

    [Test]
    public void Redo_Should_PushMutationToUndoStack()
    {
        var mutation = new Mutation("Test", () => { }, () => { });
        _store.RegisterMutation(mutation);
        _store.Undo();
        _store.Redo();

        _store.CanUndo.ShouldBeTrue();
    }

    [Test]
    public void Clear_ShouldClearUndoStack()
    {
        var mutation = new Mutation("Test", () => { }, () => { });
        _store.RegisterMutation(mutation);
        _store.Clear();
        _store.CanUndo.ShouldBeFalse();
    }

    [Test]
    public void Clear_ShouldClearRedoStack()
    {
        var mutation = new Mutation("Test", () => { }, () => { });
        _store.RegisterMutation(mutation);
        _store.Undo();
        _store.Clear();
        _store.CanRedo.ShouldBeFalse();
    }
}