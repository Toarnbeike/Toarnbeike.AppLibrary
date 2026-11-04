using Toarnbeike.AppLibrary.Entities.Audits;

namespace Toarnbeike.AppLibrary.Snapshots.Audits;

public class SoftDeleteInfoSnapshotMapperV1Tests
{
    private readonly SoftDeleteInfoSnapshotMapperV1 _mapper = new();

    private readonly SoftDeleteInfoSnapshot _snapshotData = new(
        IsDeleted: true,
        DeletedAt: new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero),
        DeletedBy: "user1");

    private readonly SoftDeleteInfo _softDeleteInfoData = new(
        IsDeleted: true,
        DeletedAt: new DateTimeOffset(2024, 6, 1, 12, 0, 0, TimeSpan.Zero),
        DeletedBy: "user1");

    [Test]
    public void ToSnapshot_ShouldMapSoftDeleteInfo_ToSnapshot()
    {
        _mapper.ToSnapshot(_softDeleteInfoData).ShouldBe(_snapshotData);
    }

    [Test]
    public void FromSnapshot_ShouldMapSnapshot_ToSoftDeleteInfo()
    {
        _mapper.FromSnapshot(_snapshotData).ShouldBe(_softDeleteInfoData);
    }

    [Test]
    public void SnapshotSchemaVersion_ShouldBe1()
    {
        SoftDeleteInfoSnapshot.SchemaVersion.ShouldBe(1);
    }
}