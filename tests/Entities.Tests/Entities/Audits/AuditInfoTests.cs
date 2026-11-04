using Toarnbeike.Optional.TestExtensions;

namespace Toarnbeike.AppLibrary.Entities.Audits;

public class AuditInfoTests
{
    [Test]
    public void Empty_AuditInfo_HasDefaultValues()
    {
        var emptyAuditInfo = AuditInfo.Empty;
        emptyAuditInfo.CreatedAt.ShouldBe(default);
        emptyAuditInfo.CreatedBy.ShouldBeNull();
        emptyAuditInfo.LastModifiedAt.ShouldBeNone();
        emptyAuditInfo.LastModifiedBy.ShouldBeNone();
    }

    [Test]
    public void WithCreated_ReturnsNewInstance_WithUpdatedCreatedInfo()
    {
        var original = AuditInfo.Empty;
        var createdAt = DateTimeOffset.UtcNow.AddDays(-1);
        var updated = original.WithCreated("newUser", createdAt);

        original.ShouldBe(AuditInfo.Empty);

        updated.CreatedAt.ShouldBe(createdAt);
        updated.CreatedBy.ShouldBe("newUser");

        updated.LastModifiedAt.ShouldBeNone();
        updated.LastModifiedBy.ShouldBeNone();
    }

    [Test]
    public void WithLastModified_ReturnsNewInstance_WithUpdatedLastModifiedInfo()
    {
        var original = AuditInfo.Empty;
        var lastModifiedAt = DateTimeOffset.UtcNow;
        var updated = original.WithLastModified("modifier", lastModifiedAt);

        original.ShouldBe(AuditInfo.Empty);

        updated.LastModifiedAt.ShouldBe(lastModifiedAt);
        updated.LastModifiedBy.ShouldBe("modifier");

        updated.CreatedAt.ShouldBe(default);
        updated.CreatedBy.ShouldBeNull();
    }
}