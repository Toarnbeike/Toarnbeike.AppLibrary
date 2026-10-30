using Toarnbeike.Results;

namespace Toarnbeike.AppLibrary.Failures;

public class EntityNotFoundFailureTests
{
    [Test]
    public void EntityNotFoundFailure_CreatesInstanceWithSearchDescription()
    {
        var searchDescription = "Test search description";

        var failure = new EntitiyNotFoundFailure<TestEntity>(searchDescription);

        failure.ShouldNotBeNull();
        failure.SearchDescription.ShouldBe(searchDescription);
        failure.Type.ShouldBe(typeof(TestEntity));
        failure.Message.ShouldBe($"Entity of type {typeof(TestEntity)} not found");
        failure.Category.ShouldBe(FailureCategory.Business);
    }

    [Test]
    public void EntityNotFoundFailure_CreatesInstanceWithEntityId()
    {
        var entityId = TestEntityId.New();

        var failure = new EntitiyNotFoundFailure<TestEntity>(entityId);

        failure.ShouldNotBeNull();
        failure.SearchDescription.ShouldBe($"For id {entityId}");
        failure.Type.ShouldBe(typeof(TestEntity));
        failure.Message.ShouldBe($"Entity of type {typeof(TestEntity)} not found");
        failure.Category.ShouldBe(FailureCategory.Business);
    }
}