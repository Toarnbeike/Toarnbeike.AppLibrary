namespace Toarnbeike.AppLibrary.Entities;

public class EntityTests
{
    [Test]
    public void EntityId_Returns_TypedIdentifier()
    {
        var id = TestEntityId.New();
        var entity = new TestEntity(id, "entity");

        entity.Id.ShouldBe(id);
        ((IEntity)entity).EntityId.ShouldBe(id);
    }
}
