using Microsoft.Extensions.DependencyInjection;
using Toarnbeike.AppLibrary.Entities.Audits;
using Toarnbeike.AppLibrary.Mutations;
using Toarnbeike.AppLibrary.Repositories;
using Toarnbeike.AppLibrary.Snapshots;
using Toarnbeike.AppLibrary.Snapshots.Audits;
using Toarnbeike.AppLibrary.Snapshots.Implementation;
using Toarnbeike.AppLibrary.Snapshots.Implementation.Validations;
using Toarnbeike.AppLibrary.State;

namespace Toarnbeike.AppLibrary.DependencyInjection;

public class DependencyInjectionExtensionTests
{
    [Test]
    public void AddStateManagement_ShouldRegisterServices()
    {
        var services = new ServiceCollection();

        services.AddStateManagement<TestSnapshotStore>();

        var serviceProvider = services.BuildServiceProvider();
        
        var stateManager = serviceProvider.GetRequiredService<IApplicationStateManager>();
        var aggregateStore = serviceProvider.GetRequiredService<IAggregateStore>();
        var mutationStore = serviceProvider.GetRequiredService<IMutationStore>();
        var snapshotStore = serviceProvider.GetRequiredService<ISnapshotStore>();

        stateManager.ShouldBeOfType<ApplicationStateManager>();
        aggregateStore.ShouldBeOfType<InMemoryAggregateStore>();
        mutationStore.ShouldBeOfType<InMemoryMutationStore>();
        snapshotStore.ShouldBeOfType<TestSnapshotStore>();
    }

    [Test]
    public void AddStateManagement_ShouldRegisterSnapshotMappers()
    {
        var services = new ServiceCollection();

        services.AddStateManagement<TestSnapshotStore>();
        
        var serviceProvider = services.BuildServiceProvider();
        
        var auditInfoMapper = serviceProvider.GetRequiredService<ISnapshotMapper<AuditInfo, AuditInfoSnapshot>>();
        var softDeleteInfoMapper = serviceProvider.GetRequiredService<ISnapshotMapper<SoftDeleteInfo, SoftDeleteInfoSnapshot>>();
        
        auditInfoMapper.ShouldBeOfType<AuditInfoSnapshotMapperV1>();
        softDeleteInfoMapper.ShouldBeOfType<SoftDeleteInfoSnapshotMapperV1>();
    }

    [Test]
    public void AddStateManagement_ShouldRegisterSnapshotRegistryValidators()
    {
        var services = new ServiceCollection();

        services.AddStateManagement<TestSnapshotStore>();
        
        var serviceProvider = services.BuildServiceProvider();
        var validators = serviceProvider.GetServices<ISnapshotRegistryValidator>().ToList();
        
        validators.ShouldContain(v => v is SnapshotDefinitionValidator);
        validators.ShouldContain(v => v is SnapshotMigratorValidator);
        validators.ShouldContain(v => v is SnapshotMigratorChainValidator);

        var registry = serviceProvider.GetRequiredService<SnapshotRegistry>();
        registry.ShouldNotBeNull();
    }

    [Test]
    public void AddRepository_ShouldRegisterRepository()
    {
        var services = new ServiceCollection();
        services.AddStateManagement<TestSnapshotStore>();

        services.AddRepository<TestAggregate, TestAggregateId>();

        var serviceProvider = services.BuildServiceProvider();
        var repository = serviceProvider.GetRequiredService<IRepository<TestAggregate, TestAggregateId>>();
        repository.ShouldBeOfType<Repository<TestAggregate, TestAggregateId>>();
    }

    [Test]
    public void AddRepository_ShouldRegisterSpecificRepository()
    {
        var services = new ServiceCollection();
        services.AddStateManagement<TestSnapshotStore>();

        services.AddRepository<TestAggregate, TestAggregateId, ITestAggregateRepository, TestAggregateRepository>();

        var serviceProvider = services.BuildServiceProvider();
        var specificRepository = serviceProvider.GetRequiredService<ITestAggregateRepository>();
        var defaultRepository = serviceProvider.GetRequiredService<IRepository<TestAggregate, TestAggregateId>>();

        specificRepository.ShouldBeOfType<TestAggregateRepository>();
        defaultRepository.ShouldBeOfType<TestAggregateRepository>();
    }

    [Test]
    public void AddSnapshot_ShouldRegisterSnapshotMapperAndDefinition()
    {
        var services = new ServiceCollection();
        services.AddStateManagement<TestSnapshotStore>();
        services.AddSnapshot<SimpleAggregate, SimpleSnapshotV1, SimpleSnapshotMapperV1>("TestResource");

        var serviceProvider = services.BuildServiceProvider();

        var mapper = serviceProvider.GetRequiredService<ISnapshotMapper<SimpleAggregate, SimpleSnapshotV1>>();
        mapper.ShouldBeOfType<SimpleSnapshotMapperV1>();

        var registry = serviceProvider.GetRequiredService<SnapshotRegistry>();
        var definition = registry.GetDefinition(typeof(SimpleAggregate));
        definition.ShouldNotBeNull();
        definition.AggregateType.ShouldBe(typeof(SimpleAggregate));
        definition.CurrentSnapshotType.ShouldBe(typeof(SimpleSnapshotV1));
    }

    [Test]
    public void AddSnapshotMigration_ShouldRegisterSnapshotMigration()
    {
        var services = new ServiceCollection();
        services.AddStateManagement<TestSnapshotStore>();
        services.AddNonAggregateMapper<SubEntity, SubEntitySnapshotV1, SubEntityMapperV1>();
        services.AddSnapshot<TestAggregate, TestSnapshotV2, TestSnapshotMapperV2>("TestResource");
        services.AddSnapshotMigration<TestSnapshotV1, TestSnapshotV2, TestSnapshotMigrationV1ToV2>();

        var serviceProvider = services.BuildServiceProvider();
        
        var migration = serviceProvider.GetRequiredService<ISnapshotMigration<TestSnapshotV1, TestSnapshotV2>>();
        migration.ShouldBeOfType<TestSnapshotMigrationV1ToV2>();

        var registry = serviceProvider.GetRequiredService<SnapshotRegistry>();
        var migrator = registry.GetMigratorForSourceType(typeof(TestSnapshotV1));
        migrator.ShouldNotBeNull();
        migrator.SourceType.ShouldBe(typeof(TestSnapshotV1));
        migrator.TargetType.ShouldBe(typeof(TestSnapshotV2));
    }

    [Test]
    public void AddNonAggregateMapper_ShouldRegisterSnapshotMapper()
    {
        var services = new ServiceCollection();
        services.AddStateManagement<TestSnapshotStore>();

        services.AddNonAggregateMapper<SubEntity, SubEntitySnapshotV1, SubEntityMapperV1>();

        var serviceProvider = services.BuildServiceProvider();

        var mapper = serviceProvider.GetRequiredService<ISnapshotMapper<SubEntity, SubEntitySnapshotV1>>();
        mapper.ShouldBeOfType<SubEntityMapperV1>();
    }

    [Test]
    public void AddNonAggregateMapper_ShouldThrowForAggregateType()
    {
        var services = new ServiceCollection();
        services.AddStateManagement<TestSnapshotStore>();

        Should.Throw<InvalidOperationException>(services.AddNonAggregateMapper<TestAggregate, TestSnapshotV2, TestSnapshotMapperV2>);
    }
}
