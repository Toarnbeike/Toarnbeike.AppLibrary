namespace Toarnbeike.AppLibrary.Mutations;

public class MutationTests
{
    private const string _originalState = "Initial State";
    private const string _modifiedState = "Modified State";

    [Test]
    public void Apply_CallsApplyAction()
    {
        var actualState = _originalState;

        var mutation = new Mutation("Test",
            () => actualState = _modifiedState,
            () => actualState = _originalState
        );

        mutation.Apply();

        actualState.ShouldBe(_modifiedState);
    }

    [Test]
    public void Revert_CallsRevertAction()
    {
        var actualState = _originalState;

        var mutation = new Mutation("Test",
            () => actualState = _modifiedState,
            () => actualState = _originalState
        );

        mutation.Apply();
        mutation.Revert();

        actualState.ShouldBe(_originalState);
    }
}