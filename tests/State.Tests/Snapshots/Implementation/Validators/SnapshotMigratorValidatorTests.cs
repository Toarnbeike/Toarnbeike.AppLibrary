using Toarnbeike.AppLibrary.Snapshots.Implementation.Validations;

namespace Toarnbeike.AppLibrary.Snapshots.Implementation.Validators;

public class SnapshotMigratorValidatorTests
{
    [Test]
    public void Validate_ShouldThrowException_ForDuplicateSourceTypes()
    {
        var migrators = new List<ISnapshotMigrator>
        {
            new SnapshotMigrator<TestSnapshotV1, TestSnapshotV2>(Substitute.For<ISnapshotMigration<TestSnapshotV1, TestSnapshotV2>>()),
            new SnapshotMigrator<TestSnapshotV1, SimpleSnapshotV1>(Substitute.For<ISnapshotMigration<TestSnapshotV1, SimpleSnapshotV1>>()),
        };
        var validator = new SnapshotMigratorValidator(migrators);

        var exception = Assert.Throws<InvalidOperationException>(validator.Validate);
        
        exception.Message.ShouldContain($"Duplicate snapshot migrators found for source types:");
        exception.Message.ShouldContain($"TestSnapshotV1");
    }

    [Test]
    public void Validate_ShouldThrowException_ForDuplicateTargetTypes()
    {
        var migrators = new List<ISnapshotMigrator>
        {
            new SnapshotMigrator<TestSnapshotV1, TestSnapshotV2>(Substitute.For<ISnapshotMigration<TestSnapshotV1, TestSnapshotV2>>()),
            new SnapshotMigrator<SimpleSnapshotV1, TestSnapshotV2>(Substitute.For<ISnapshotMigration<SimpleSnapshotV1, TestSnapshotV2>>()),
        };
        var validator = new SnapshotMigratorValidator(migrators);

        var exception = Assert.Throws<InvalidOperationException>(validator.Validate);

        exception.Message.ShouldContain($"Duplicate snapshot migrators found for target types:");
        exception.Message.ShouldContain($"TestSnapshotV2");
    }

    [Test]
    public void Validate_ShouldThrowException_ForInvalidSourceTypeSchemaVersions()
    {
        var migrators = new List<ISnapshotMigrator>
        {
            new SnapshotMigrator<SimpleSnapshotV0, SimpleSnapshotV1>(Substitute.For<ISnapshotMigration<SimpleSnapshotV0, SimpleSnapshotV1>>()),
        };
        var validator = new SnapshotMigratorValidator(migrators);
        
        var exception = Assert.Throws<InvalidOperationException>(validator.Validate);
        
        exception.Message.ShouldContain($"Snapshot migrators found with invalid schema versions (less than or equal to zero) for source-target type pairs:");
        exception.Message.ShouldContain($"Toarnbeike.AppLibrary.SimpleSnapshotV0 -> Toarnbeike.AppLibrary.SimpleSnapshotV1");
    }

    [Test]
    public void Validate_ShouldThrowException_ForInvalidTargetTypeSchemaVersions()
    {
        var migrators = new List<ISnapshotMigrator>
        {
            new SnapshotMigrator<SimpleSnapshotV1, SimpleSnapshotV0>(Substitute.For<ISnapshotMigration<SimpleSnapshotV1, SimpleSnapshotV0>>()),
        };
        var validator = new SnapshotMigratorValidator(migrators);

        var exception = Assert.Throws<InvalidOperationException>(validator.Validate);

        exception.Message.ShouldContain($"Snapshot migrators found with invalid schema versions (less than or equal to zero) for source-target type pairs:");
        exception.Message.ShouldContain($"Toarnbeike.AppLibrary.SimpleSnapshotV1 -> Toarnbeike.AppLibrary.SimpleSnapshotV0");
    }

    [Test]
    public void Validate_ShouldThrowException_ForInvalidIncrementalSchemaVersions()
    {
        var migrators = new List<ISnapshotMigrator>
        {
            new SnapshotMigrator<SimpleSnapshotV1, SimpleSnapshotV3>(Substitute.For<ISnapshotMigration<SimpleSnapshotV1, SimpleSnapshotV3>>()),
        };
        var validator = new SnapshotMigratorValidator(migrators);
        var exception = Assert.Throws<InvalidOperationException>(validator.Validate);
        exception.Message.ShouldContain($"Snapshot migrators found with non-incremental schema versions for source-target type pairs:");
        exception.Message.ShouldContain($"Toarnbeike.AppLibrary.SimpleSnapshotV1 (v1) -> Toarnbeike.AppLibrary.SimpleSnapshotV3 (v3)");
    }

    [Test]
    public void Validate_ShouldThrowException_ForInvalidIncrementalSchemaVersions_ReverseOrder()
    {
        var migrators = new List<ISnapshotMigrator>
        {
            new SnapshotMigrator<TestSnapshotV2, TestSnapshotV1>(Substitute.For<ISnapshotMigration<TestSnapshotV2, TestSnapshotV1>>()),
        };
        var validator = new SnapshotMigratorValidator(migrators);
        var exception = Assert.Throws<InvalidOperationException>(validator.Validate);
        exception.Message.ShouldContain($"Snapshot migrators found with non-incremental schema versions for source-target type pairs:");
        exception.Message.ShouldContain($"Toarnbeike.AppLibrary.TestSnapshotV2 (v2) -> Toarnbeike.AppLibrary.TestSnapshotV1 (v1)");
    }

    [Test]
    public void Validate_ShouldThrowException_ForInvalidIncrementalSchemaVersions_EqualVersions()
    {
        var migrators = new List<ISnapshotMigrator>
        {
            new SnapshotMigrator<SimpleSnapshotV1, SimpleSnapshotV1>(Substitute.For<ISnapshotMigration<SimpleSnapshotV1, SimpleSnapshotV1>>()),
        };
        var validator = new SnapshotMigratorValidator(migrators);
        var exception = Assert.Throws<InvalidOperationException>(validator.Validate);
        exception.Message.ShouldContain($"Snapshot migrators found with non-incremental schema versions for source-target type pairs:");
        exception.Message.ShouldContain($"Toarnbeike.AppLibrary.SimpleSnapshotV1 (v1) -> Toarnbeike.AppLibrary.SimpleSnapshotV1 (v1)");
    }
}