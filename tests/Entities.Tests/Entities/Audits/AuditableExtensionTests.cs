using Toarnbeike.Optional.TestExtensions;

namespace Toarnbeike.AppLibrary.Entities.Audits;

public class AuditableExtensionTests
{
    [Test]
    public void WithCreated_ReturnsNewInstance_WithUpdatedCreatedInfo()
    {
        var entity = new TestEntity(TestEntityId.New(), "entity");
        var createdAt = DateTimeOffset.UtcNow.AddDays(-1);

        var updatedEntity = entity.WithCreated("newUser", createdAt);

        entity.AuditInfo.ShouldBe(AuditInfo.Empty);

        updatedEntity.AuditInfo.CreatedAt.ShouldBe(createdAt);
        updatedEntity.AuditInfo.CreatedBy.ShouldBe("newUser");
        updatedEntity.AuditInfo.LastModifiedAt.ShouldBeNone();
        updatedEntity.AuditInfo.LastModifiedBy.ShouldBeNone();
    }

    [Test]
    public void WithLastModified_ReturnsNewInstance_WithUpdatedLastModifiedInfo()
    {
        var entity = new TestEntity(TestEntityId.New(), "entity");
        var lastModifiedAt = DateTimeOffset.UtcNow;
        
        var updatedEntity = entity.WithLastModified("modifier", lastModifiedAt);

        entity.AuditInfo.ShouldBe(AuditInfo.Empty);
        
        updatedEntity.AuditInfo.LastModifiedAt.ShouldBe(lastModifiedAt);
        updatedEntity.AuditInfo.LastModifiedBy.ShouldBe("modifier");
        updatedEntity.AuditInfo.CreatedAt.ShouldBe(default);
        updatedEntity.AuditInfo.CreatedBy.ShouldBeNull();
    }
}
