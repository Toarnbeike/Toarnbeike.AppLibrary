using Toarnbeike.AppLibrary.Snapshots.Implementation.Validations;

namespace Toarnbeike.AppLibrary.Snapshots.Implementation.Validators;

public class SnapshotDefinitionValidatorTests
{
    [Test]
    public void Validate_ShouldThrowException_ForDuplicateAggregateTypes()
    {
        var definitions = new List<ISnapshotDefinition>
        {
            new SnapshotDefinition<TestAggregate, TestSnapshotV1>("testV1", Substitute.For<ISnapshotMapper<TestAggregate, TestSnapshotV1>>()),
            new SnapshotDefinition<TestAggregate, TestSnapshotV2>("testV2", Substitute.For<ISnapshotMapper<TestAggregate, TestSnapshotV2>>()),
            new SnapshotDefinition<SimpleAggregate, SimpleSnapshotV1>("simpleV1", Substitute.For<ISnapshotMapper<SimpleAggregate, SimpleSnapshotV1>>()),
            new SnapshotDefinition<SimpleAggregate, SimpleSnapshotV1>("simpleOther", Substitute.For<ISnapshotMapper<SimpleAggregate, SimpleSnapshotV1>>()),
        };

        var validator = new SnapshotDefinitionValidator(definitions);

        var exception = Assert.Throws<InvalidOperationException>(validator.Validate);

        exception.Message.ShouldContain("Duplicate snapshot definitions found for aggregate types:");
        exception.Message.ShouldContain("TestAggregate");
        exception.Message.ShouldContain("SimpleAggregate");
    }

    [Test]
    public void Validate_ShouldThrowException_ForDuplicateResourceNames()
    {
        var definitions = new List<ISnapshotDefinition>
        {
            new SnapshotDefinition<TestAggregate, TestSnapshotV1>("match", Substitute.For<ISnapshotMapper<TestAggregate, TestSnapshotV1>>()),
            new SnapshotDefinition<SimpleAggregate, SimpleSnapshotV1>("match", Substitute.For<ISnapshotMapper<SimpleAggregate, SimpleSnapshotV1>>()),
        };
        var validator = new SnapshotDefinitionValidator(definitions);

        var exception = Assert.Throws<InvalidOperationException>(validator.Validate);

        exception.Message.ShouldContain("Duplicate snapshot definitions found for resource names:");
        exception.Message.ShouldContain("match");
    }

    [Test]
    public void Validate_ShouldThrowException_ForInvalidSchemaVersions()
    {
        var definitions = new List<ISnapshotDefinition>
        {
            new SnapshotDefinition<SimpleAggregate, SimpleSnapshotV0>("simpleV0", Substitute.For<ISnapshotMapper<SimpleAggregate, SimpleSnapshotV0>>()),
        };
        var validator = new SnapshotDefinitionValidator(definitions);

        var exception = Assert.Throws<InvalidOperationException>(validator.Validate);

        exception.Message.ShouldContain("Snapshot definitions found with invalid schema versions (less than or equal to zero) for resource names:");
        exception.Message.ShouldContain("simpleV0");
    }
}
