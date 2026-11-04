using Toarnbeike.AppLibrary.Snapshots.Implementation.Validations;

namespace Toarnbeike.AppLibrary.Snapshots.Implementation.Validators;

public class SnapshotMigratorChainValidatorTests
{
    [Test]
    public void Validate_ShouldThrowException_ForMissingTargetDefinitions()
    {
        var definitions = new List<ISnapshotDefinition>
        {
            new SnapshotDefinition<TestAggregate, TestSnapshotV1>("testV1", Substitute.For<ISnapshotMapper<TestAggregate, TestSnapshotV1>>()),
        };

        var migrators = new List<ISnapshotMigrator>
        {
            new SnapshotMigrator<TestSnapshotV1, TestSnapshotV2>(Substitute.For<ISnapshotMigration<TestSnapshotV1, TestSnapshotV2>>()),
        };
        var validator = new SnapshotMigratorChainValidator(definitions, migrators);

        var exception = Assert.Throws<InvalidOperationException>(validator.Validate);

        exception.Message.ShouldContain("Snapshot migrators found with missing target definitions for snapshot types:");
        exception.Message.ShouldContain("TestSnapshotV2");
    }

    [Test]
    public void Validate_ShouldThrowException_ForMigratorsWithoutSourceDefinitions()
    {
        var definitions = new List<ISnapshotDefinition>()
        {
            new SnapshotDefinition<TestAggregate, TestSnapshotV1>("testV1", Substitute.For<ISnapshotMapper<TestAggregate, TestSnapshotV1>>()),
            new SnapshotDefinition<TestAggregate, TestSnapshotV2>("testV2", Substitute.For<ISnapshotMapper<TestAggregate, TestSnapshotV2>>()),
        };

        var migrators = new List<ISnapshotMigrator>
        {
            new SnapshotMigrator<TestSnapshotV1, TestSnapshotV2>(Substitute.For<ISnapshotMigration<TestSnapshotV1, TestSnapshotV2>>()),
            new SnapshotMigrator<TestSnapshotV2, TestSnapshotV1>(Substitute.For<ISnapshotMigration<TestSnapshotV2, TestSnapshotV1>>()),
        };
        var validator = new SnapshotMigratorChainValidator(definitions, migrators);

        var exception = Assert.Throws<InvalidOperationException>(validator.Validate);

        exception.Message.ShouldContain("Snapshot migrators found that are not part of any migration chain leading to a current snapshot version:");
        exception.Message.ShouldContain("Toarnbeike.AppLibrary.TestSnapshotV2 -> Toarnbeike.AppLibrary.TestSnapshotV1");
    }
}