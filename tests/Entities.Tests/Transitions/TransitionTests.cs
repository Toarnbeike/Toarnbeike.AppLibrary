namespace Toarnbeike.AppLibrary.Transitions;

public class TransitionTests
{
    [Test]
    public void Created_Transition_ShouldHaveCorrectProperties()
    {
        var entity = new TestEntity(TestEntityId.New(), "Test Name");

        var transition = Transition<TestEntity>.Created(entity);
        
        transition.Value.ShouldBe(entity);
        transition.Type.ShouldBe(TransitionType.Created);
        transition.Events.ShouldBeEmpty();
    }

    [Test]
    public void Created_Transition_WithEvents_ShouldHaveCorrectProperties()
    {
        var entity = new TestEntity(TestEntityId.New(), "Test Name");
        var event1 = new TestEvent(TestEntityId.New());
        var event2 = new TestEvent(TestEntityId.New());

        var transition = Transition<TestEntity>.Created(entity, event1, event2);

        transition.Events.Count.ShouldBe(2);
        transition.Events[0].ShouldBe(event1);
        transition.Events[1].ShouldBe(event2);
    }

    [Test]
    public void Modified_Transition_ShouldHaveCorrectProperties()
    {
        var entity = new TestEntity(TestEntityId.New(), "Test Name");

        var transition = Transition<TestEntity>.Modified(entity);

        transition.Value.ShouldBe(entity);
        transition.Type.ShouldBe(TransitionType.Modified);
        transition.Events.ShouldBeEmpty();
    }

    [Test]
    public void Modified_Transition_WithEvents_ShouldHaveCorrectProperties()
    {
        var entity = new TestEntity(TestEntityId.New(), "Test Name");
        var event1 = new TestEvent(TestEntityId.New());
        var event2 = new TestEvent(TestEntityId.New());

        var transition = Transition<TestEntity>.Modified(entity, event1, event2);

        transition.Events.Count.ShouldBe(2);
        transition.Events[0].ShouldBe(event1);
        transition.Events[1].ShouldBe(event2);
    }

    [Test]
    public void Deleted_Transition_ShouldHaveCorrectProperties()
    {
        var entity = new TestEntity(TestEntityId.New(), "Test Name");

        var transition = Transition<TestEntity>.Deleted(entity);

        transition.Value.ShouldBe(entity);
        transition.Type.ShouldBe(TransitionType.Deleted);
        transition.Events.ShouldBeEmpty();
    }

    [Test]
    public void Deleted_Transition_WithEvents_ShouldHaveCorrectProperties()
    {
        var entity = new TestEntity(TestEntityId.New(), "Test Name");
        var event1 = new TestEvent(TestEntityId.New());
        var event2 = new TestEvent(TestEntityId.New());

        var transition = Transition<TestEntity>.Deleted(entity, event1, event2);

        transition.Events.Count.ShouldBe(2);
        transition.Events[0].ShouldBe(event1);
        transition.Events[1].ShouldBe(event2);
    }

    [Test]
    public void AddEvents_ShouldAppendEventsToExistingTransition()
    {
        var entity = new TestEntity(TestEntityId.New(), "Test Name");
        var initialEvent = new TestEvent(TestEntityId.New());
        var additionalEvent1 = new TestEvent(TestEntityId.New());
        var additionalEvent2 = new TestEvent(TestEntityId.New());
        var transition = Transition<TestEntity>.Created(entity, initialEvent);

        var updatedTransition = transition.AddEvents(additionalEvent1, additionalEvent2);

        transition.Events.Count.ShouldBe(1);
        updatedTransition.Events.Count.ShouldBe(3);
        updatedTransition.Events[0].ShouldBe(initialEvent);
        updatedTransition.Events[1].ShouldBe(additionalEvent1);
        updatedTransition.Events[2].ShouldBe(additionalEvent2);
    }
}
