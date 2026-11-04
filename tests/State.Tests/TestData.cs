using Toarnbeike.AppLibrary.Entities;
using Toarnbeike.AppLibrary.Repositories;
using Toarnbeike.AppLibrary.Snapshots;
using Toarnbeike.AppLibrary.State;
using Toarnbeike.Results;

namespace Toarnbeike.AppLibrary;

public readonly record struct TestAggregateId(Guid Value) : IEntityId;
public sealed record TestAggregate(TestAggregateId Id, SubEntity SubEntity) : IAggregate<TestAggregateId>
{
    public static TestAggregate CreateNew(SubEntity subEntity) => new(new TestAggregateId(Guid.NewGuid()), subEntity);
}

public readonly record struct SubEntityId(Guid Value) : IEntityId;

public sealed record SubEntity(SubEntityId Id, string Data) : IEntity<SubEntityId>
{
    public static SubEntity CreateNew(string data) => new(new SubEntityId(Guid.NewGuid()), data);
}

public interface ITestAggregateRepository : IRepository<TestAggregate, TestAggregateId>;
public sealed class TestAggregateRepository(IAggregateStore store) : Repository<TestAggregate, TestAggregateId>(store), ITestAggregateRepository;

public record TestSnapshotV1(Guid Id) : ISnapshot
{
    public static int SchemaVersion => 1;
}

public record TestSnapshotV2(Guid Id, SubEntitySnapshotV1 SubEntity) : ISnapshot
{
    public static int SchemaVersion => 2;
}

public sealed record SubEntitySnapshotV1(Guid Id, string Data) : ISnapshot
{
    public static int SchemaVersion => 1;
}

public sealed class SubEntityMapperV1 : ISnapshotMapper<SubEntity, SubEntitySnapshotV1>
{
    public SubEntitySnapshotV1 ToSnapshot(SubEntity entity) => new(entity.Id.Value, entity.Data);
    public SubEntity FromSnapshot(SubEntitySnapshotV1 snapshot) => new(new SubEntityId(snapshot.Id), snapshot.Data);
}

public sealed class TestSnapshotMapperV2(ISnapshotMapper<SubEntity, SubEntitySnapshotV1> subEntityMapper) : ISnapshotMapper<TestAggregate, TestSnapshotV2>
{
    public TestSnapshotV2 ToSnapshot(TestAggregate aggregate) => new(aggregate.Id.Value, subEntityMapper.ToSnapshot(aggregate.SubEntity));
    public TestAggregate FromSnapshot(TestSnapshotV2 snapshot) => new(new TestAggregateId(snapshot.Id), subEntityMapper.FromSnapshot(snapshot.SubEntity));
}

public sealed class TestSnapshotMigrationV1ToV2 : ISnapshotMigration<TestSnapshotV1, TestSnapshotV2>
{
    public TestSnapshotV2 Migrate(TestSnapshotV1 snapshot) => new(snapshot.Id, new SubEntitySnapshotV1(Guid.NewGuid(), "Migrated data"));
}

public sealed class TestSnapshotStore : ISnapshotStore
{
    public string GetLatestState() => "Latest state";
    public string GetState(string identifier) => "Latest state";
    public IEnumerable<string> GetAvailableIdentifiers() => ["Latest state"];

    public Result Read(string state, IAggregateStore aggregateStore) => Result.Success();
    public Result WriteAll(IAggregateStore aggregateStore) => Result.Success();
}

public readonly record struct SimpleAggregateId(Guid Value) : IEntityId;

public sealed record SimpleAggregate(SimpleAggregateId Id) : IAggregate<SimpleAggregateId>
{
    public static SimpleAggregate CreateNew() => new(new SimpleAggregateId(Guid.NewGuid()));
}

public sealed record SimpleSnapshotV1(Guid Id) : ISnapshot
{
    public static int SchemaVersion => 1;
}

public sealed record SimpleSnapshotMapperV1 : ISnapshotMapper<SimpleAggregate, SimpleSnapshotV1>
{
    public SimpleSnapshotV1 ToSnapshot(SimpleAggregate aggregate) => new(aggregate.Id.Value);
    public SimpleAggregate FromSnapshot(SimpleSnapshotV1 snapshot) => new(new SimpleAggregateId(snapshot.Id));
}

public sealed record SimpleSnapshotV0 : ISnapshot
{
    public static int SchemaVersion => 0;
}

public sealed record SimpleSnapshotV3 : ISnapshot
{
    public static int SchemaVersion => 3;
}