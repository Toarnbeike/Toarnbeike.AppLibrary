namespace Toarnbeike.AppLibrary.Snapshots.Implementation;

public class SnapshotMigratorTests
{
    private readonly ISnapshotMigration<TestSnapshotV1, TestSnapshotV2> _migration = new TestSnapshotMigrationV1ToV2();
    private readonly SnapshotMigrator<TestSnapshotV1, TestSnapshotV2> _migrator;

    public SnapshotMigratorTests()
    {
        _migrator = new SnapshotMigrator<TestSnapshotV1, TestSnapshotV2>(_migration);
    }

    [Test]
    public void SourceType_Should_EqualOldSnapshotType()
    {
        _migrator.SourceType.ShouldBe(typeof(TestSnapshotV1));
    }

    [Test]
    public void TargetType_Should_EqualNewSnapshotType()
    {
        _migrator.TargetType.ShouldBe(typeof(TestSnapshotV2));
    }

    [Test]
    public void SourceVersion_Should_EqualOldSnapshotVersion()
    {
        _migrator.SourceVersion.ShouldBe(TestSnapshotV1.SchemaVersion);
    }

    [Test]
    public void TargetVersion_Should_EqualNewSnapshotVersion()
    {
        _migrator.TargetVersion.ShouldBe(TestSnapshotV2.SchemaVersion);
    }

    [Test]
    public void Migrate_Should_ConvertOldSnapshotToNewSnapshot()
    {
        var oldSnapshot = new TestSnapshotV1(Guid.NewGuid());
        var oldDocument = new SnapshotDocument<TestSnapshotV1>("TestResource", [oldSnapshot]);

        var newDocument = _migrator.Migrate(oldDocument);

        var castedDocument = newDocument.ShouldBeOfType<SnapshotDocument<TestSnapshotV2>>();
        castedDocument.ResourceName.ShouldBe("TestResource");
        castedDocument.Snapshots.Length.ShouldBe(1);

        var newSnapshot = castedDocument.Snapshots[0];
        var castedSnapshot = newSnapshot.ShouldBeOfType<TestSnapshotV2>();
        castedSnapshot.SubEntity.Data.ShouldBe("Migrated data");
    }
}
