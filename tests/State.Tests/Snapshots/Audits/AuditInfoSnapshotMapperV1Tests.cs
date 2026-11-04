using Toarnbeike.AppLibrary.Entities.Audits;

namespace Toarnbeike.AppLibrary.Snapshots.Audits;

public class AuditInfoSnapshotMapperV1Tests
{
    private readonly AuditInfoSnapshotMapperV1 _mapper = new();
    private readonly AuditInfoSnapshot _snapshotData = new(
        CreatedBy: "user1",
        CreatedAt: new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero),
        LastModifiedBy: "user2",
        LastModifiedAt: new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero));

    private readonly AuditInfo _auditInfoData = new(
        CreatedBy: "user1",
        CreatedAt: new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero),
        LastModifiedBy: "user2",
        LastModifiedAt: new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero));

    [Test]
    public void ToSnapshot_ShouldMapAuditInfo_ToSnapshot()
    {
        _mapper.ToSnapshot(_auditInfoData).ShouldBe(_snapshotData);
    }

    [Test]
    public void FromSnapshot_ShouldMapSnapshot_ToAuditInfo()
    {
        _mapper.FromSnapshot(_snapshotData).ShouldBe(_auditInfoData);
    }

    [Test]
    public void SnapshotSchemaVersion_ShouldBe1()
    {
        AuditInfoSnapshot.SchemaVersion.ShouldBe(1);
    }
}