namespace Toarnbeike.AppLibrary.Entities.Audits;

public class SoftDeletableExtensionTests
{
    [Test]
    public void WithSoftDeleted_ReturnsNewInstance_WithUpdatedSoftDeleteInfo()
    {
        var entity = new TestEntity(TestEntityId.New(), "entity");
        var deletedAt = DateTimeOffset.UtcNow;

        var updatedEntity = entity.WithSoftDeleted("deleter", deletedAt);
        entity.SoftDeleteInfo.ShouldBe(SoftDeleteInfo.NotDeleted);

        updatedEntity.SoftDeleteInfo.IsDeleted.ShouldBe(true);
        updatedEntity.SoftDeleteInfo.DeletedBy.ShouldBe("deleter");
        updatedEntity.SoftDeleteInfo.Deleted.ShouldBe(deletedAt);
    }
}
