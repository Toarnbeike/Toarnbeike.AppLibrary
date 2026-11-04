namespace Toarnbeike.AppLibrary.Mutations;

public class MutationBatchTests
{
    [Test]
    public void MutationBatch_Apply_CallsActionsInOrder()
    {
        var appliedActions = new List<string>();
        var revertedActions = new List<string>();
        var mutation1 = new Mutation("Mutation1",
            () => appliedActions.Add("Mutation1 Applied"),
            () => revertedActions.Add("Mutation1 Reverted"));
        var mutation2 = new Mutation("Mutation2",
            () => appliedActions.Add("Mutation2 Applied"),
            () => revertedActions.Add("Mutation2 Reverted"));

        var batch = new MutationBatch([mutation1, mutation2], "Batch");
        batch.Apply();
        appliedActions.ShouldBe(["Mutation1 Applied", "Mutation2 Applied"]);
        batch.MutationDescription.ShouldBe("Batch");
    }

    [Test]
    public void MutationBatch_Revert_CallsActionsInReverseOrder()
    {
        var appliedActions = new List<string>();
        var revertedActions = new List<string>();
        var mutation1 = new Mutation("Mutation1",
            () => appliedActions.Add("Mutation1 Applied"),
            () => revertedActions.Add("Mutation1 Reverted"));
        var mutation2 = new Mutation("Mutation2",
            () => appliedActions.Add("Mutation2 Applied"),
            () => revertedActions.Add("Mutation2 Reverted"));

        var batch = new MutationBatch([mutation1, mutation2], "Batch");

        batch.Revert();
        revertedActions.ShouldBe(["Mutation2 Reverted", "Mutation1 Reverted"]);
    }
}
