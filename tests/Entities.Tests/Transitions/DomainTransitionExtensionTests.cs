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

        var transition = entity.Created(entity => new TestEvent(entity.Id), entity => new TestEvent(entity.Id));

        transition.Events.Count.ShouldBe(2);
        transition.Events[0].ShouldBeOfType<TestEvent>().Id.ShouldBe(entity.Id);
        transition.Events[1].ShouldBeOfType<TestEvent>().Id.ShouldBe(entity.Id);
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

        var transition = entity.Modified(entity => new TestEvent(entity.Id), entity => new TestEvent(entity.Id));

        transition.Events.Count.ShouldBe(2);
        transition.Events[0].ShouldBeOfType<TestEvent>().Id.ShouldBe(entity.Id);
        transition.Events[1].ShouldBeOfType<TestEvent>().Id.ShouldBe(entity.Id);
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

        var transition = entity.Deleted(entity => new TestEvent(entity.Id), entity => new TestEvent(entity.Id));

        transition.Events.Count.ShouldBe(2);
        transition.Events[0].ShouldBeOfType<TestEvent>().Id.ShouldBe(entity.Id);
        transition.Events[1].ShouldBeOfType<TestEvent>().Id.ShouldBe(entity.Id);
    }

    [Test]
    public void CreatedDerived_Transition_ShouldHaveCorrectProperties()
    {
        var entity = new DerivedTestEntity(TestEntityId.New(), "Test Name", 42);

        var transition = entity.Created<DerivedTestEntity, TestEntity>();

        transition.ShouldBeOfType<Transition<TestEntity>>();
        transition.Value.ShouldBe(entity);
        transition.Type.ShouldBe(TransitionType.Created);
        transition.Events.ShouldBeEmpty();
    }

    [Test]
    public void CreatedDerived_Transition_WithEvents_ShouldHaveCorrectProperties()
    {
        var entity = new DerivedTestEntity(TestEntityId.New(), "Test Name", 42);

        var transition = entity.Created<DerivedTestEntity, TestEntity>(entity => new TestEvent(entity.Id), entity => new TestEvent(entity.Id));

        transition.Events.Count.ShouldBe(2);
        transition.Events[0].ShouldBeOfType<TestEvent>().Id.ShouldBe(entity.Id);
        transition.Events[1].ShouldBeOfType<TestEvent>().Id.ShouldBe(entity.Id);
    }

    [Test]
    public void ModifiedDerived_Transition_ShouldHaveCorrectProperties()
    {
        var entity = new DerivedTestEntity(TestEntityId.New(), "Test Name", 42);

        var transition = entity.Modified<DerivedTestEntity, TestEntity>();

        transition.ShouldBeOfType<Transition<TestEntity>>();
        transition.Value.ShouldBe(entity);
        transition.Type.ShouldBe(TransitionType.Modified);
        transition.Events.ShouldBeEmpty();
    }

    [Test]
    public void ModifiedDerived_Transition_WithEvents_ShouldHaveCorrectProperties()
    {
        var entity = new DerivedTestEntity(TestEntityId.New(), "Test Name", 42);

        var transition = entity.Modified<DerivedTestEntity, TestEntity>(entity => new TestEvent(entity.Id), entity => new TestEvent(entity.Id));

        transition.Events.Count.ShouldBe(2);
        transition.Events[0].ShouldBeOfType<TestEvent>().Id.ShouldBe(entity.Id);
        transition.Events[1].ShouldBeOfType<TestEvent>().Id.ShouldBe(entity.Id);
    }

    [Test]
    public void DeletedDerived_Transition_ShouldHaveCorrectProperties()
    {
        var entity = new DerivedTestEntity(TestEntityId.New(), "Test Name", 42);

        var transition = entity.Deleted<DerivedTestEntity, TestEntity>();

        transition.ShouldBeOfType<Transition<TestEntity>>();
        transition.Value.ShouldBe(entity);
        transition.Type.ShouldBe(TransitionType.Deleted);
        transition.Events.ShouldBeEmpty();
    }

    [Test]
    public void DeletedDerived_Transition_WithEvents_ShouldHaveCorrectProperties()
    {
        var entity = new DerivedTestEntity(TestEntityId.New(), "Test Name", 42);

        var transition = entity.Deleted<DerivedTestEntity, TestEntity>(entity => new TestEvent(entity.Id), entity => new TestEvent(entity.Id));

        transition.Events.Count.ShouldBe(2);
        transition.Events[0].ShouldBeOfType<TestEvent>().Id.ShouldBe(entity.Id);
        transition.Events[1].ShouldBeOfType<TestEvent>().Id.ShouldBe(entity.Id);
    }
}
