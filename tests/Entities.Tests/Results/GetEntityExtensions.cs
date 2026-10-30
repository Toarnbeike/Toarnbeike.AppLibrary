using Toarnbeike.AppLibrary.Failures;
using Toarnbeike.Results.TestExtensions;

namespace Toarnbeike.AppLibrary.Results;

internal class GetEntityExtensions
{
    private readonly TestEntity _entity1 = new(TestEntityId.New(), "Entity 1");
    private readonly TestEntity _entity2 = new(TestEntityId.New(), "Entity 2");
    private readonly DerivedTestEntity _derivedEntity = new(TestEntityId.New(), "Derived Entity", 42);
    private readonly IEnumerable<TestEntity> _entities;

    public GetEntityExtensions()
    {
        _entities = [_entity1, _entity2, _derivedEntity];
    }

    [Test]
    public void GetEntity_ByExistingId_ReturnsSuccessResultWithEntity()
    {
        var result = _entities.GetEntity(_entity1.Id);

        result.ShouldBeSuccess().ShouldBe(_entity1);
    }

    [Test]
    public void GetEntity_ByNonExistingId_ReturnsFailureResult()
    {
        var nonExistingId = TestEntityId.New();

        var result = _entities.GetEntity(nonExistingId);
        
        var failure = result.ShouldBeFailureOfType<EntitiyNotFoundFailure<TestEntity>>();
        failure.SearchDescription.ShouldBe($"By id '{nonExistingId}'");
    }

    [Test]
    public void GetEntity_ByExistingDerivedEntityId_ReturnsSuccessResultWithDerivedEntity()
    {
        var result = _entities.GetEntity(_derivedEntity.Id);

        result.ShouldBeSuccess().ShouldBe(_derivedEntity);
    }

    [Test]
    public void GetEntity_ByPredicateMatchingEntity_ReturnsSuccessResultWithEntity()
    {
        var result = _entities.GetEntity(e => e.Name == "Entity 2", "Name equals 'Entity 2'");

        result.ShouldBeSuccess().ShouldBe(_entity2);
    }

    [Test]
    public void GetEntity_ByPredicateNotMatchingAnyEntity_ReturnsFailureResult()
    {
        var result = _entities.GetEntity(e => e.Name == "Non-Existing Entity", "Name equals 'Non-Existing Entity'");

        var failure = result.ShouldBeFailureOfType<EntitiyNotFoundFailure<TestEntity>>();

        failure.SearchDescription.ShouldBe("Name equals 'Non-Existing Entity'");
    }

    [Test]
    public void GetEntity_ByPredicateMatchingDerivedEntity_ReturnsSuccessResultWithDerivedEntity()
    {
        var result = _entities.GetEntity(e => e.Name == "Derived Entity", "Name equals 'Derived Entity'");

        result.ShouldBeSuccess().ShouldBe(_derivedEntity);
    }
}
