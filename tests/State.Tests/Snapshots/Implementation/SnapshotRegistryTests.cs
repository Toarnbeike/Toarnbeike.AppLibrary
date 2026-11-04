using Microsoft.Extensions.DependencyInjection;
using Toarnbeike.AppLibrary.DependencyInjection;

namespace Toarnbeike.AppLibrary.Snapshots.Implementation;

public class SnapshotRegistryTests
{
    private readonly SnapshotRegistry _registry;

    public SnapshotRegistryTests()
    {
        var services = new ServiceCollection();
        services.AddStateManagement<TestSnapshotStore>();
        services.AddNonAggregateMapper<SubEntity, SubEntitySnapshotV1, SubEntityMapperV1>();
        services.AddSnapshot<TestAggregate, TestSnapshotV2, TestSnapshotMapperV2>("TestResource");
        services.AddSnapshotMigration<TestSnapshotV1, TestSnapshotV2, TestSnapshotMigrationV1ToV2>();
        services.AddSnapshot<SimpleAggregate, SimpleSnapshotV1, SimpleSnapshotMapperV1>("SimpleResource");

        var serviceProvider = services.BuildServiceProvider();
        _registry = serviceProvider.GetRequiredService<SnapshotRegistry>();
    }

    [Test]
    public void GetDefinitionByType_ShouldReturnDefinition_WhenExists()
    {
        var definition = _registry.GetDefinition(typeof(SimpleAggregate));
        definition.ShouldNotBeNull();
        definition.AggregateType.ShouldBe(typeof(SimpleAggregate));
        definition.CurrentSnapshotType.ShouldBe(typeof(SimpleSnapshotV1));
        definition.CurrentSchemaVersion.ShouldBe(1);
        definition.ResourceName.ShouldBe("SimpleResource");
    }

    [Test]
    public void GetDefinitionByType_ShouldReturnNull_WhenNotExists()
    {
        var definition = _registry.GetDefinition(typeof(string));
        definition.ShouldBeNull();
    }

    [Test]
    public void GetDefinitionByResourceName_ShouldReturnDefinition_WhenExists()
    {
        var definition = _registry.GetDefinition("SimpleResource");
        definition.ShouldNotBeNull();
        definition.AggregateType.ShouldBe(typeof(SimpleAggregate));
    }

    [Test]
    public void GetDefinitionByResourceName_ShouldIgnoreCase()
    {
        var definition = _registry.GetDefinition("simpleresource");
        definition.ShouldNotBeNull();
        definition.AggregateType.ShouldBe(typeof(SimpleAggregate));
    }

    [Test]
    public void GetMigratorForSourceType_ShouldReturnMigrator_WhenExists()
    {
        var migrator = _registry.GetMigratorForSourceType(typeof(TestSnapshotV1));
        migrator.ShouldNotBeNull();
        migrator.SourceType.ShouldBe(typeof(TestSnapshotV1));
        migrator.TargetType.ShouldBe(typeof(TestSnapshotV2));
        migrator.SourceVersion.ShouldBe(1);
        migrator.TargetVersion.ShouldBe(2);
    }

    [Test]
    public void GetMigratorForSourceType_ShouldReturnNull_WhenNotExists()
    {
        var migrator = _registry.GetMigratorForSourceType(typeof(string));
        migrator.ShouldBeNull();
    }

    [Test]
    public void GetDefinitionByResourceName_ShouldReturnNull_WhenNotExists()
    {
        var definition = _registry.GetDefinition("NonExistentResource");
        definition.ShouldBeNull();
    }

    [Test]
    public void GetSnapshotType_ShouldReturnCurrentType_WhenRequestedVersionIsCurrent()
    {
        var definition = _registry.GetDefinition(typeof(TestAggregate)).ShouldNotBeNull();
        var snapshotType = _registry.GetSnapshotType(definition, 2);
        snapshotType.ShouldBe(typeof(TestSnapshotV2));
    }

    [Test]
    public void GetSnapshotType_ShouldReturnPreviousType_WhenRequestedVersionIsPrevious()
    {
        var definition = _registry.GetDefinition(typeof(TestAggregate)).ShouldNotBeNull();
        var snapshotType = _registry.GetSnapshotType(definition, 1);
        snapshotType.ShouldBe(typeof(TestSnapshotV1));
    }

    [Test]
    public void GetSnapshotType_ShouldReturnCurrentType_WhenOnlyOneVersionExists()
    {
        var definition = _registry.GetDefinition(typeof(SimpleAggregate)).ShouldNotBeNull();
        var snapshotType = _registry.GetSnapshotType(definition, 1);
        snapshotType.ShouldBe(typeof(SimpleSnapshotV1));
    }

    [Test]
    public void GetSnapshotType_ShouldReturnNull_WhenRequestedVersionIsInvalid()
    {
        var definition = _registry.GetDefinition(typeof(TestAggregate)).ShouldNotBeNull();
        var snapshotType = _registry.GetSnapshotType(definition, 3);
        snapshotType.ShouldBeNull();
    }

    [Test]
    public void GetSnapshotType_ShouldReturnNull_WhenRequestedVersionIsZeroOrNegative()
    {
        var definition = _registry.GetDefinition(typeof(TestAggregate)).ShouldNotBeNull();
        var snapshotTypeZero = _registry.GetSnapshotType(definition, 0);
        var snapshotTypeNegative = _registry.GetSnapshotType(definition, -1);
        snapshotTypeZero.ShouldBeNull();
        snapshotTypeNegative.ShouldBeNull();
    }

    [Test]
    public void GetSnapshotType_ShouldReturnNull_WhenNoMigratorExistsForRequestedVersion()
    {
        var services = new ServiceCollection();
        services.AddStateManagement<TestSnapshotStore>();
        services.AddNonAggregateMapper<SubEntity, SubEntitySnapshotV1, SubEntityMapperV1>();
        services.AddSnapshot<TestAggregate, TestSnapshotV2, TestSnapshotMapperV2>("TestResource");

        var serviceProvider = services.BuildServiceProvider();
        var registry = serviceProvider.GetRequiredService<SnapshotRegistry>();

        var definition = registry.GetDefinition(typeof(TestAggregate)).ShouldNotBeNull();
        var snapshotType = registry.GetSnapshotType(definition, 1);
        snapshotType.ShouldBeNull();
    }
}