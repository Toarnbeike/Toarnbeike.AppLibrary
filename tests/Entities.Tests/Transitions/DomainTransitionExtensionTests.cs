namespace Toarnbeike.AppLibrary.Transitions;

public class DomainTransitionExtensionTests
{
    [Test]
    public void Created_Transition_ShouldHaveCorrectProperties()
    {
        var entity = new TestEntity(TestEntityId.New(), "Test Name");

        var transition = entity.Created();

        transition.Value.ShouldBe(entity);
        transition.Type.ShouldBe(TransitionType.Created);
        transition.Events.ShouldBeEmpty();
    }

    [Test]
    public void Created_Transition_WithEvents_ShouldHaveCorrectProperties()
    {
        var entity = new TestEntity(TestEntityId.New(), "Test Name");
        var event1 = new TestEvent(Guid.NewGuid());
        var event2 = new TestEvent(Guid.NewGuid());

        var transition = entity.Created(event1, event2);

        transition.Events.Count.ShouldBe(2);
        transition.Events[0].ShouldBe(event1);
        transition.Events[1].ShouldBe(event2);
    }

    [Test]
    public void Modified_Transition_ShouldHaveCorrectProperties()
    {
        var entity = new TestEntity(TestEntityId.New(), "Test Name");

        var transition = entity.Modified();

        transition.Value.ShouldBe(entity);
        transition.Type.ShouldBe(TransitionType.Modified);
        transition.Events.ShouldBeEmpty();
    }

    [Test]
    public void Modified_Transition_WithEvents_ShouldHaveCorrectProperties()
    {
        var entity = new TestEntity(TestEntityId.New(), "Test Name");
        var event1 = new TestEvent(Guid.NewGuid());
        var event2 = new TestEvent(Guid.NewGuid());

        var transition = entity.Modified(event1, event2);

        transition.Events.Count.ShouldBe(2);
        transition.Events[0].ShouldBe(event1);
        transition.Events[1].ShouldBe(event2);
    }

    [Test]
    public void Deleted_Transition_ShouldHaveCorrectProperties()
    {
        var entity = new TestEntity(TestEntityId.New(), "Test Name");

        var transition = entity.Deleted();

        transition.Value.ShouldBe(entity);
        transition.Type.ShouldBe(TransitionType.Deleted);
        transition.Events.ShouldBeEmpty();
    }

    [Test]
    public void Deleted_Transition_WithEvents_ShouldHaveCorrectProperties()
    {
        var entity = new TestEntity(TestEntityId.New(), "Test Name");
        var event1 = new TestEvent(Guid.NewGuid());
        var event2 = new TestEvent(Guid.NewGuid());

        var transition = entity.Deleted(event1, event2);

        transition.Events.Count.ShouldBe(2);
        transition.Events[0].ShouldBe(event1);
        transition.Events[1].ShouldBe(event2);
    }
}
