using Toarnbeike.AppLibrary.Snapshots.Implementation;
using Toarnbeike.AppLibrary.Snapshots.Implementation.Validations;

namespace Toarnbeike.AppLibrary.DependencyInjection;

public class SnapshotRegistryBuilderTests
{
    [Test]
    public void Build_ShouldCallAllValidators()
    {
        var testValidator1 = Substitute.For<ISnapshotRegistryValidator>();
        var testValidator2 = Substitute.For<ISnapshotRegistryValidator>();

        var definitions = new List<ISnapshotDefinition>();
        var migrators = new List<ISnapshotMigrator>();
        var validators = new List<ISnapshotRegistryValidator> { testValidator1, testValidator2 };

        var builder = new SnapshotRegistryBuilder(validators, definitions, migrators);
        builder.Build();

        testValidator1.Received().Validate();
        testValidator2.Received().Validate();
    }
}