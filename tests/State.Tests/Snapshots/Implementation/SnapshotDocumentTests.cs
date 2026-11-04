namespace Toarnbeike.AppLibrary.Snapshots.Implementation;

public class SnapshotDocumentTests
{
    [Test]
    public void SnapshotDocument_Should_StoreSnapshotsAndResourceName()
    {
        var snapshot1 = new TestSnapshotV1(Guid.NewGuid());
        var snapshot2 = new TestSnapshotV1(Guid.NewGuid());
        var snapshots = new[] { snapshot1, snapshot2 };
        var resourceName = "TestResource";

        var document = new SnapshotDocument<TestSnapshotV1>(resourceName, snapshots);

        document.SnapshotType.ShouldBe(typeof(TestSnapshotV1));
        document.ResourceName.ShouldBe(resourceName);
        document.SchemaVersion.ShouldBe(TestSnapshotV1.SchemaVersion);
        document.Snapshots.Length.ShouldBe(2);
        document.Snapshots[0].ShouldBe(snapshot1);
        document.Snapshots[1].ShouldBe(snapshot2);

        var interfaceDocument = (ISnapshotDocument)document;
        interfaceDocument.ResourceName.ShouldBe(resourceName);
        interfaceDocument.Collection.ShouldNotBeNull();
        interfaceDocument.Collection.ShouldBeOfType(typeof(TestSnapshotV1[]));
    }
}
