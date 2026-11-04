using Toarnbeike.AppLibrary.Repositories;

namespace Toarnbeike.AppLibrary.Snapshots.Implementation;

public class SnapshotDefinitionTests
{
    private const string ResourceName = "TestResource";
    private readonly ISnapshotMapper<TestAggregate, TestSnapshotV2> _mapper = 
        Substitute.For<ISnapshotMapper<TestAggregate, TestSnapshotV2>>();
    private readonly SnapshotDefinition<TestAggregate, TestSnapshotV2> _definition;

    public SnapshotDefinitionTests()
    {
        _definition = new SnapshotDefinition<TestAggregate, TestSnapshotV2>(ResourceName, _mapper);
    }

    [Test]
    public void AggregateType_Should_EqualAggregateType()
    {
        _definition.AggregateType.ShouldBe(typeof(TestAggregate));
    }

    [Test]
    public void CurrentSnapshotType_Should_EqualSnapshotType()
    {
        _definition.CurrentSnapshotType.ShouldBe(typeof(TestSnapshotV2));
    }

    [Test]
    public void CurrentSchemaVersion_Should_EqualSnapshotSchemaVersion()
    {
        _definition.CurrentSchemaVersion.ShouldBe(TestSnapshotV2.SchemaVersion);
    }

    [Test]
    public void ResourceName_Should_EqualResourceName()
    {
        _definition.ResourceName.ShouldBe(ResourceName);
    }

    [Test]
    public void CreateSnapshot_Should_CreateSnapshotDocumentWithMappedSnapshots()
    {
        var store = Substitute.For<IAggregateStore>();
        var subEntity = SubEntity.CreateNew("TestData");
        var aggregate = TestAggregate.CreateNew(subEntity);
        store.GetAll<TestAggregate>().Returns([aggregate]);
        _mapper.ToSnapshot(aggregate).Returns(
            new TestSnapshotV2(aggregate.Id.Value, new SubEntitySnapshotV1(subEntity.Id.Value, subEntity.Data)));

        var document = _definition.CreateSnapshot(store);

        document.ShouldNotBeNull();
        var castedDocument = document.ShouldBeOfType<SnapshotDocument<TestSnapshotV2>>();
        castedDocument.ResourceName.ShouldBe(ResourceName);

        castedDocument.Snapshots.Length.ShouldBe(1);
        var snapshot = castedDocument.Snapshots[0];
        snapshot.Id.ShouldBe(aggregate.Id.Value);
    }

    [Test]
    public void LoadSnapshots_Should_LoadAggregatesFromSnapshotDocument()
    {
        var store = Substitute.For<IAggregateStore>();
        var subEntitySnapshot = new SubEntitySnapshotV1(Guid.NewGuid(), "TestData");
        var snapshot = new TestSnapshotV2(Guid.NewGuid(), subEntitySnapshot);
        var document = new SnapshotDocument<TestSnapshotV2>(ResourceName, [snapshot]);
        _mapper.FromSnapshot(Arg.Any<TestSnapshotV2>())
            .Returns(TestAggregate.CreateNew(SubEntity.CreateNew("data")));

        _definition.LoadSnapshots(store, document);

        store.Received(1).SetAll(Arg.Is<TestAggregate[]>(aggregates =>
            aggregates.Length == 1 &&
            aggregates[0].SubEntity.Data == "data"));
    }
}